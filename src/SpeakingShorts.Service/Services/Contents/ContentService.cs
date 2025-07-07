
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
using SpeakingShorts.Service.Services.UserActivities;

namespace SpeakingShorts.Service.Services.Contents;

public class ContentService(IUnitOfWork _unitOfWork, ILogger<ContentService> _logger, IBackblazeService _storageService, IUserActivityService _userActivityService) : IContentService
{
     private readonly string _baseUrl = "https://f005.backblazeb2.com/file/Speaking/";
    public async Task<Content> CreateAndProcessAsync(IFormFile file, ContentType type, string title)
    {
        try
        {
            // Validate file
            if (file == null || file.Length == 0)
                throw new FileUploadException("File is empty or null");

            var currentUserId = HttpContextHelper.GetUserId;
            var today = DateTime.UtcNow.Date; // Kun boshidan boshlab

            // UserActivity ni tekshirish
            var userActivity = await _unitOfWork.UserActivityRepository
                .SelectAsync(ua => ua.UserId == currentUserId && ua.UploadDate.Date == today);

            if (userActivity != null && !userActivity.IsContentDeleted)
            {
                throw new ForbiddenException("You can only create one content per day. Delete the existing content to upload a new one.");
            }

            var fileKey = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            await using var stream = file.OpenReadStream();
            var fileUrl = await _storageService.UploadFileAsync(stream, fileKey, file.ContentType);

            var content = new Content
            {
                Title = title,
                UserId = currentUserId,
                Type = type,
                FileKey = fileKey,
                FileUrl = fileUrl,
                FileSize = file.Length,
                Status = ContentStatus.Ready,
                CreatedById = currentUserId
            };

            var createdContent = await _unitOfWork.ContentRepository.InsertAsync(content);
            await _unitOfWork.SaveAsync();

            // UserActivity ni yangilash yoki yaratish
            if (userActivity == null)
            {
                userActivity = new UserActivity
                {
                    UserId = currentUserId,
                    UploadDate = DateTime.UtcNow,
                    IsContentDeleted = false,
                    CreatedById = currentUserId
                };
                await _userActivityService.CreateAsync(userActivity);
            }
            else
            {
                userActivity.IsContentDeleted = false;
                await _userActivityService.ModifyAsync(userActivity.Id, userActivity);
            }

            _logger.LogInformation("Content created successfully: {Id}", createdContent.Id);
            return createdContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating and processing content");
            throw;
        }
    }

    public async ValueTask<Content> GetAsync(long id)
    {
        try
        {
            var content =  await _unitOfWork.ContentRepository.SelectAsync(c => c.Id == id, includes: ["User", "Comments", "Likes"])
                   ?? throw new NotFoundException("Content not found.");
             content.FileUrl = _baseUrl + content.FileKey;
            return content;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting content: {Id}", id);
            throw;
        }
    }

    public async ValueTask<IEnumerable<Content>> GetAllAsync()
    {
        try
        {
            var contents =  await _unitOfWork.ContentRepository.Select(includes: ["User", "Comments", "Likes"]).ToListAsync();
            foreach (var content in contents)
            {
                content.FileUrl = _baseUrl + content.FileKey;
            }
            return contents;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all contents");
            throw;
        }
    }

    public async ValueTask DeleteAsync(long id)
    {
        try
        {
            var content = await _unitOfWork.ContentRepository.SelectAsync(c => c.Id == id)
                          ?? throw new NotFoundException("Content not found.");

            if (content.UserId != HttpContextHelper.GetUserId)
                throw new ForbiddenException("You are not allowed to delete this content");

            await _storageService.DeleteFileAsync(content.FileKey);

            content.DeletedById = HttpContextHelper.GetUserId;
            content.DeletedAt = DateTime.UtcNow;
            await _unitOfWork.ContentRepository.DeleteAsync(content);
            await _unitOfWork.SaveAsync();

            // Content o'chirilganda UserActivity ni yangilash
            var userActivity = await _unitOfWork.UserActivityRepository
                .SelectAsync(ua => ua.UserId == HttpContextHelper.GetUserId && ua.UploadDate.Date == DateTime.UtcNow.Date);
            if (userActivity != null)
            {
                await _userActivityService.MarkContentAsDeletedAsync(content.Id);
            }

            _logger.LogInformation("Content deleted successfully: {Id}", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting content: {Id}", id);
            throw;
        }
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
            var query = _unitOfWork.ContentRepository.Select(isTracking: false, includes: ["User", "Comments", "Likes"]);

            if (!string.IsNullOrWhiteSpace(search))
                query = query.Where(c => 
                    c.Title.ToLower().Contains(search.ToLower()) ||
                    c.Description.ToLower().Contains(search.ToLower()));

            var contents =  await query.ToPaginateAsQueryable(@params).OrderBy(filter).ToListAsync();
            foreach (var content in contents)
            {
              content.FileUrl = _baseUrl + content.FileKey; 
            }
            return contents;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting paginated contents");
            throw;
        }
    }
}