using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.MarkedWords;

namespace SpeakingShorts.WebApi.ApiService.MarkedWords
{
    public interface IMarkedWordApiService
    {
        ValueTask<MarkedWordViewModel> CreateAsync(MarkedWordCreateModel model);
        ValueTask<MarkedWordViewModel> ModifyAsync(long id, MarkedWordModifyModel model);
        ValueTask<bool> DeleteAsync(long id);
        ValueTask<MarkedWordViewModel> GetAsync(long id);
        ValueTask<IEnumerable<MarkedWordViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
        ValueTask<IEnumerable<MarkedWordViewModel>> GetAllAsync();
        ValueTask<IEnumerable<MarkedWordViewModel>> GetByStoryIdAsync(long storyId);
    }
}