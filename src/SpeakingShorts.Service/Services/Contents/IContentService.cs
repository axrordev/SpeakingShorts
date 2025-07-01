using Microsoft.AspNetCore.Http;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Domain.Entities.Enums;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.Contents;

public interface IContentService
{
    Task<Content> CreateAndProcessAsync(IFormFile file, ContentType type, string title);
    ValueTask<Content> GetAsync(long id);
    ValueTask<IEnumerable<Content>> GetAllAsync();
    ValueTask<IEnumerable<Content>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<Content> ModifyAsync(long id, Content content);
    ValueTask DeleteAsync(long id);
} 