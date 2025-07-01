using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.Stories;

namespace SpeakingShorts.WebApi.ApiService.Stories
{
    // Interface (agar hali mavjud bo'lmasa)
    public interface IStoryApiService
    {
        ValueTask<StoryViewModel> CreateAsync(StoryCreateModel model);
        ValueTask<StoryViewModel> ModifyAsync(long id, StoryModifyModel model);
        ValueTask<bool> DeleteAsync(long id);
        ValueTask<StoryViewModel> GetAsync(long id);
        ValueTask<IEnumerable<StoryViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
        ValueTask<IEnumerable<StoryViewModel>> GetAllAsync();
    }
}
