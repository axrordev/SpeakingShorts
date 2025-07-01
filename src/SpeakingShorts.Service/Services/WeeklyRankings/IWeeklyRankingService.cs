using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;

public interface IWeeklyRankingService
{
    Task GenerateWeeklyRankingsAsync();
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<WeeklyRanking> GetAsync(long id);
    ValueTask<IEnumerable<WeeklyRanking>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<WeeklyRanking>> GetAllAsync();
    ValueTask<IEnumerable<WeeklyRanking>> GetByUserIdAsync(long userId);
    ValueTask<WeeklyRanking> GetCurrentWeekRankingAsync();
    ValueTask<WeeklyRanking> GetLastWeekRankingAsync();
    ValueTask<WeeklyRanking> GetByWeekNumberAsync(int weekNumber);
}