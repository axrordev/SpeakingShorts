using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Exceptions;
using SpeakingShorts.Service.Helpers;
using SpeakingShorts.Service.Services.BackblazeServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace SpeakingShorts.Service.Services.BackgroundMusics
{
    public class BackgroundMusicService : IBackgroundMusicService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<BackgroundMusicService> _logger;
        private readonly IBackblazeService _storageService;

        public BackgroundMusicService(
            IUnitOfWork unitOfWork,
            ILogger<BackgroundMusicService> logger,
            IBackblazeService storageService)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _storageService = storageService;
        }

        public async Task<BackgroundMusic> CreateAsync(IFormFile file, string title)
        {
            var fileKey = $"background-music/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            await using var stream = file.OpenReadStream();
            var fileUrl = await _storageService.UploadFileAsync(stream, fileKey, file.ContentType);

            var music = new BackgroundMusic
            {
                Title = title,
                FileKey = fileKey,
                FileUrl = fileUrl,
                FileSize = file.Length,
                IsActive = true,
                CreatedById = HttpContextHelper.GetUserId
            };

            var createdMusic = await _unitOfWork.BackgroundMusicRepository.InsertAsync(music);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Background music created successfully: {Title}", title);
            return createdMusic;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var music = await _unitOfWork.BackgroundMusicRepository.SelectAsync(m => m.Id == id)
                        ?? throw new NotFoundException("Background music not found.");

            await _storageService.DeleteFileAsync(music.FileKey);
            await _unitOfWork.BackgroundMusicRepository.DeleteAsync(music);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Background music deleted successfully: {Id}", id);
            return true;
        }

        public async Task<BackgroundMusic> GetAsync(long id)
        {
            return await _unitOfWork.BackgroundMusicRepository.SelectAsync(m => m.Id == id)
                   ?? throw new NotFoundException("Background music not found.");
        }

        public async Task<IEnumerable<BackgroundMusic>> GetAllAsync()
        {
            return await _unitOfWork.BackgroundMusicRepository.Select().ToListAsync();
        }
    }
} 