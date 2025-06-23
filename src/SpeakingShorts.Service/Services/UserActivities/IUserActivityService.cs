using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.UserActivities;

public interface IUserActivityService
{
    ValueTask<UserActivity> CreateAsync(UserActivity userActivity);
    ValueTask<UserActivity> ModifyAsync(long id, UserActivity userActivity);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<UserActivity> GetAsync(long id);
    ValueTask<IEnumerable<UserActivity>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<UserActivity>> GetAllAsync();
    ValueTask<IEnumerable<UserActivity>> GetByUserIdAsync(long userId);
    ValueTask<UserActivity> MarkContentAsDeletedAsync(long contentId);
} 