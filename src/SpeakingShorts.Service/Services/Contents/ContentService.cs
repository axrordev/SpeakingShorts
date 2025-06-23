using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Enums;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Extensions;
using SpeakingShorts.Service.Helpers;
using SpeakingShorts.Service.Services.BackblazeServices;
using SpeakingShorts.Service.Services.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SpeakingShorts.Service.Services.Contents;

public class ContentService : IContentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ContentService> _logger;
    private readonly IBackblazeService _storageService;
    private readonly IBackgroundTaskQueue _taskQueue;
    private readonly IVideoProcessingService _videoProcessor;

    public ContentService(
        IUnitOfWork unitOfWork,
        ILogger<ContentService> logger,
        IBackblazeService storageService,
        IBackgroundTaskQueue taskQueue,
        IVideoProcessingService videoProcessor)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _storageService = storageService ?? throw new ArgumentNullException(nameof(storageService));
        _taskQueue = taskQueue ?? throw new ArgumentNullException(nameof(taskQueue));
        _videoProcessor = videoProcessor ?? throw new ArgumentNullException(nameof(videoProcessor));
    }

    public async Task<Content> CreateAndProcessAsync(IFormFile file, ContentType type, string title, long? backgroundMusicId)
    {
        var fileKey = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        await using var stream = file.OpenReadStream();
        var fileUrl = await _storageService.UploadFileAsync(stream, fileKey, file.ContentType);

        var content = new Content
        {
            Title = title,
            UserId =HttpContextHelper.GetUserId,
            Type = type,
            FileKey = fileKey,
            FileUrl = fileUrl,
            FileSize = file.Length,
            Status = backgroundMusicId.HasValue ? ContentStatus.Processing : ContentStatus.Ready,
            BackgroundMusicId = backgroundMusicId,
            CreatedById = HttpContextHelper.GetUserId
        };

        if (!backgroundMusicId.HasValue)
        {
            content.ResultFileKey = fileKey;
        }

        var createdContent = await _unitOfWork.ContentRepository.InsertAsync(content);
        await _unitOfWork.SaveAsync();

        if (backgroundMusicId.HasValue)
        {
            _taskQueue.QueueBackgroundWorkItem(async token =>
            {
                await _videoProcessor.ProcessVideoAsync(createdContent.Id, backgroundMusicId);
            });
        }

        return createdContent;
    }

    public async Task ProcessContentAsync(long contentId, long? backgroundMusicId)
    {
        var content = await _unitOfWork.ContentRepository.SelectAsync(c => c.Id == contentId)
                      ?? throw new NotFoundException("Content not found.");

        content.Status = ContentStatus.Processing;
        content.BackgroundMusicId = backgroundMusicId;
        await _unitOfWork.ContentRepository.UpdateAsync(content);
        await _unitOfWork.SaveAsync();

        _taskQueue.QueueBackgroundWorkItem(async token =>
        {
            await _videoProcessor.ProcessVideoAsync(contentId, backgroundMusicId);
        });
    }

    public async ValueTask<Content> GetAsync(long id)
    {
        return await _unitOfWork.ContentRepository.SelectAsync(c => c.Id == id, includes: new[] { "BackgroundMusic" })
               ?? throw new NotFoundException("Content not found.");
    }

    public async ValueTask<IEnumerable<Content>> GetAllAsync()
    {
        return await _unitOfWork.ContentRepository.Select(includes: new[] { "BackgroundMusic" }).ToListAsync();
    }

    public async ValueTask DeleteAsync(long id)
    {
        var content = await _unitOfWork.ContentRepository.SelectAsync(c => c.Id == id)
                      ?? throw new NotFoundException("Content not found.");

        if (content.UserId != HttpContextHelper.GetUserId)
            throw new ForbiddenException("You are not allowed to delete this content");
                
        await _storageService.DeleteFileAsync(content.FileKey);
        if (!string.IsNullOrEmpty(content.ResultFileKey))
        {
            await _storageService.DeleteFileAsync(content.ResultFileKey);
        }

        content.DeletedById = HttpContextHelper.GetUserId;
        await _unitOfWork.ContentRepository.DeleteAsync(content);
        await _unitOfWork.SaveAsync();
    }

    public async ValueTask<Content> ModifyAsync(long id, Content content)
    {
        try
        {
            var existContent = await _unitOfWork.ContentRepository.SelectAsync(c => c.Id == id)
                ?? throw new NotFoundException("Content not found");

            if (existContent.UserId != HttpContextHelper.GetUserId)
                throw new ForbiddenException("You are not allowed to modify this content");

            existContent.Title = content.Title;
            existContent.Description = content.Description;
            existContent.UpdatedById = HttpContextHelper.GetUserId;
            existContent.UpdatedAt = DateTime.UtcNow;

            var updatedContent = await _unitOfWork.ContentRepository.UpdateAsync(existContent);
            await _unitOfWork.SaveAsync();
            
            _logger.LogInformation("Content updated successfully: {Id}", id);
            return updatedContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating content: {Id}", id);
            throw;
        }
    }

    public async ValueTask<IEnumerable<Content>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
    {
        try
        {
            var contents = _unitOfWork.ContentRepository.Select(isTracking: false, includes: ["User", "Comments", "Likes", "BackgroundMusic"]);

            if (!string.IsNullOrWhiteSpace(search))
                contents = contents.Where(c => 
                    c.Title.ToLower().Contains(search.ToLower()) ||
                    c.Description.ToLower().Contains(search.ToLower()));

            return await contents.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated contents");
            throw;
        }
    }

    /// <summary>
    /// Sets background music for content
    /// </summary>
    public async ValueTask<Content> SetBackgroundMusicAsync(long contentId, long? backgroundMusicId)
    {
        try
        {
            _logger.LogInformation("Setting background music for content: {ContentId}, BackgroundMusicId: {BackgroundMusicId}", 
                contentId, backgroundMusicId);

            var existingContent = await _unitOfWork.ContentRepository.SelectAsync(c => c.Id == contentId)
                ?? throw new NotFoundException("Content not found");

            if (existingContent.UserId != HttpContextHelper.GetUserId)
                throw new ForbiddenException("You are not allowed to modify this content");

            // If background music ID is provided, validate it exists and is active
            if (backgroundMusicId.HasValue)
            {
                var backgroundMusic = await _unitOfWork.BackgroundMusicRepository
                    .SelectAsync(b => b.Id == backgroundMusicId.Value && b.IsActive && !b.IsDeleted)
                    ?? throw new NotFoundException("Background music not found or not active");
            }

            existingContent.BackgroundMusicId = backgroundMusicId;
            existingContent.UpdatedById = HttpContextHelper.GetUserId;
            existingContent.UpdatedAt = DateTime.UtcNow;

            var updatedContent = await _unitOfWork.ContentRepository.UpdateAsync(existingContent);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Background music set successfully for content: {ContentId}", contentId);

            return updatedContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error setting background music for content: {ContentId}", contentId);
            throw;
        }
    }

    /// <summary>
    /// Removes background music from content
    /// </summary>
    public async ValueTask<Content> RemoveBackgroundMusicAsync(long contentId)
    {
        try
        {
            _logger.LogInformation("Removing background music from content: {ContentId}", contentId);

            var existingContent = await _unitOfWork.ContentRepository.SelectAsync(c => c.Id == contentId)
                ?? throw new NotFoundException("Content not found");

            if (existingContent.UserId != HttpContextHelper.GetUserId)
                throw new ForbiddenException("You are not allowed to modify this content");

            existingContent.BackgroundMusicId = null;
            existingContent.UpdatedById = HttpContextHelper.GetUserId;
            existingContent.UpdatedAt = DateTime.UtcNow;

            var updatedContent = await _unitOfWork.ContentRepository.UpdateAsync(existingContent);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Background music removed successfully from content: {ContentId}", contentId);

            return updatedContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error removing background music from content: {ContentId}", contentId);
            throw;
        }
    }
} 