using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.Likes;

public interface ILikeService
{
    ValueTask<Like> CreateAsync(Like like);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<Like> GetAsync(long id);
    ValueTask<IEnumerable<Like>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<Like>> GetAllAsync();
    ValueTask<IEnumerable<Like>> GetByContentIdAsync(long contentId);
    ValueTask<bool> ToggleLikeAsync(long contentId);
} 