using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.WeeklyRankings;

public interface IWeeklyRankingApiService
{
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<WeeklyRankingViewModel> GetAsync(long id);
    ValueTask<IEnumerable<WeeklyRankingViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<WeeklyRankingViewModel>> GetAllAsync();
    ValueTask<IEnumerable<WeeklyRankingViewModel>> GetByUserIdAsync(long userId);
    ValueTask<WeeklyRankingViewModel> GetCurrentWeekRankingAsync();
    ValueTask<WeeklyRankingViewModel> GetLastWeekRankingAsync();
    ValueTask<WeeklyRankingViewModel> GetByWeekNumberAsync(int weekNumber);
}