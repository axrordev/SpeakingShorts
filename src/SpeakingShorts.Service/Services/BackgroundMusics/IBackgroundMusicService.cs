using SpeakingShorts.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace SpeakingShorts.Service.Services.BackgroundMusics
{
    public interface IBackgroundMusicService
    {
        Task<BackgroundMusic> CreateAsync(IFormFile file, string title);
        Task<bool> DeleteAsync(long id);
        Task<BackgroundMusic> GetAsync(long id);
        Task<IEnumerable<BackgroundMusic>> GetAllAsync();
    }
} 