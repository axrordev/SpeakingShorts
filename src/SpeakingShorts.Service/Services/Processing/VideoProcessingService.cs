using Microsoft.Extensions.Logging;
using SpeakingShorts.Data.UnitOfWorks;
using SpeakingShorts.Service.Services.BackblazeServices;
using System;
using System.IO;
using System.Threading.Tasks;
using Xabe.FFmpeg;
using SpeakingShorts.Domain.Entities.Enums;
using SpeakingShorts.Service.Exceptions;
using System.Linq;

namespace SpeakingShorts.Service.Services.Processing
{
    public class VideoProcessingService : IVideoProcessingService
    {
        private readonly ILogger<VideoProcessingService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBackblazeService _storageService;

        public VideoProcessingService(
            ILogger<VideoProcessingService> logger,
            IUnitOfWork unitOfWork,
            IBackblazeService storageService)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _storageService = storageService;
        }

        public async Task ProcessVideoAsync(long contentId, long? backgroundMusicId)
        {
            var content = await _unitOfWork.ContentRepository.SelectAsync(c => c.Id == contentId)
                          ?? throw new NotFoundException("Content not found for processing.");

            var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempDir);
            
            string mainFilePath = string.Empty;
            string musicFilePath = null;
            string outputFilePath = Path.Combine(tempDir, $"{Guid.NewGuid()}{Path.GetExtension(content.FileKey)}");

            try
            {
                // 1. Download files
                _logger.LogInformation("Downloading files for content ID: {ContentId}", contentId);
                mainFilePath = await _storageService.DownloadFileToPathAsync(content.FileKey, Path.Combine(tempDir, content.FileKey));

                if (backgroundMusicId.HasValue)
                {
                    var music = await _unitOfWork.BackgroundMusicRepository.SelectAsync(m => m.Id == backgroundMusicId.Value)
                                ?? throw new NotFoundException("Background music not found.");
                    musicFilePath = await _storageService.DownloadFileToPathAsync(music.FileKey, Path.Combine(tempDir, music.FileKey));
                }

                // 2. Process with FFmpeg
               if (musicFilePath != null)
{
    _logger.LogInformation("Merging content with background music for content ID: {ContentId}", contentId);
    
    // Asosiy oqim (video/audio)
    IStream videoStream = await FFmpeg.GetMediaInfo(mainFilePath).ContinueWith(t => t.Result.VideoStreams.FirstOrDefault());
    IStream audioStream = await FFmpeg.GetMediaInfo(mainFilePath).ContinueWith(t => t.Result.AudioStreams.FirstOrDefault());

    // Musiqa oqimi
    IStream musicAudioStream = await FFmpeg.GetMediaInfo(musicFilePath).ContinueWith(t => t.Result.AudioStreams.FirstOrDefault());

    var conversion = FFmpeg.Conversions.New()
        .AddStream(videoStream) // Asosiy video oqimini qo'shamiz
        .AddStream(audioStream) // Asosiy audio oqimini qo'shamiz
        .AddStream(musicAudioStream) // Musiqa oqimini qo'shamiz
        .SetOutput(outputFilePath)
        // Ovoz balandligini sozlash va birlashtirish
        .AddParameter($"-filter_complex \"[1:a]volume=1[a0];[2:a]volume=0.2[a1];[a0][a1]amix=inputs=2:duration=longest\" -c:v copy");
    
    await conversion.Start();
}
                else
                {
                    _logger.LogInformation("No background music. Copying original file for content ID: {ContentId}", contentId);
                    File.Copy(mainFilePath, outputFilePath);
                }

                // 3. Upload result
                _logger.LogInformation("Uploading processed file for content ID: {ContentId}", contentId);
                var resultFileKey = $"processed_{Guid.NewGuid()}{Path.GetExtension(outputFilePath)}";
                await using var resultStream = new FileStream(outputFilePath, FileMode.Open, FileAccess.Read);
                var resultUrl = await _storageService.UploadFileAsync(resultStream, resultFileKey, "video/mp4"); // Adjust content type if needed

                // 4. Update database
                content.Status = ContentStatus.Ready;
                content.ResultFileKey = resultFileKey;
                await _unitOfWork.ContentRepository.UpdateAsync(content);
                await _unitOfWork.SaveAsync();
                _logger.LogInformation("Successfully processed and updated content ID: {ContentId}", contentId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing content ID: {ContentId}", contentId);
                content.Status = ContentStatus.Failed;
                await _unitOfWork.ContentRepository.UpdateAsync(content);
                await _unitOfWork.SaveAsync();
            }
            finally
            {
                // 5. Cleanup temporary files
                if (Directory.Exists(tempDir))
                {
                    Directory.Delete(tempDir, true);
                }
            }
        }
    }
} 