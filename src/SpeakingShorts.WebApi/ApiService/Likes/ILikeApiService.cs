using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.Likes;

namespace SpeakingShorts.WebApi.ApiService.Likes
{
    public interface ILikeApiService
    {
        ValueTask<LikeViewModel> CreateAsync(LikeCreateModel model);
        ValueTask<bool> DeleteAsync(long id);
        ValueTask<LikeViewModel> GetAsync(long id);
        ValueTask<IEnumerable<LikeViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
        ValueTask<IEnumerable<LikeViewModel>> GetAllAsync();
        ValueTask<IEnumerable<LikeViewModel>> GetByContentIdAsync(long contentId);
        ValueTask<bool> ToggleLikeAsync(long contentId);
    }
}
