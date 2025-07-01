using SpeakingShorts.Domain.Entities.Enums;
using SpeakingShorts.Service.Configurations;

public interface IContentApiService
{
        Task<ContentViewModel> CreateAndProcessAsync(IFormFile file, ContentType type, string title);
        ValueTask<ContentViewModel> GetAsync(long id);
        ValueTask<IEnumerable<ContentViewModel>> GetAllAsync();
        ValueTask<IEnumerable<ContentViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
        ValueTask<ContentViewModel> ModifyAsync(long id, ContentModifyModel model);
        ValueTask DeleteAsync(long id);
    }