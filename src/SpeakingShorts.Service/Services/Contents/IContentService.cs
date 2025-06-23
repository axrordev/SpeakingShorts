using Microsoft.AspNetCore.Http;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Enums;
using SpeakingShorts.Service.Configurations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeakingShorts.Service.Services.Contents;

public interface IContentService
{
    Task<Content> CreateAndProcessAsync(IFormFile file, ContentType type, string title, long? backgroundMusicId);
    ValueTask<Content> GetAsync(long id);
    ValueTask<IEnumerable<Content>> GetAllAsync();
    ValueTask DeleteAsync(long id);
    
    /// <summary>
    /// Sets background music for content
    /// </summary>
    /// <param name="contentId">Content ID</param>
    /// <param name="backgroundMusicId">Background music ID</param>
    /// <returns>Updated content</returns>
    ValueTask<Content> SetBackgroundMusicAsync(long contentId, long? backgroundMusicId);
    
    /// <summary>
    /// Removes background music from content
    /// </summary>
    /// <param name="contentId">Content ID</param>
    /// <returns>Updated content</returns>
    ValueTask<Content> RemoveBackgroundMusicAsync(long contentId);
} 