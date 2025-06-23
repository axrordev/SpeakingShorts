using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.Stories;

public interface IStoryService
{
    ValueTask<Story> CreateAsync(Story story);
    ValueTask<Story> ModifyAsync(long id, Story story);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<Story> GetAsync(long id);
    ValueTask<IEnumerable<Story>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<Story>> GetAllAsync();
} 