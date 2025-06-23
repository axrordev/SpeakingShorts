using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;

namespace SpeakingShorts.Service.Services.WeeklyRankings;

public interface IWeeklyRankingService
{
    ValueTask<WeeklyRanking> CreateAsync(WeeklyRanking weeklyRanking);
    ValueTask<WeeklyRanking> ModifyAsync(long id, WeeklyRanking weeklyRanking);
    ValueTask<bool> DeleteAsync(long id);
    ValueTask<WeeklyRanking> GetAsync(long id);
    ValueTask<IEnumerable<WeeklyRanking>> GetAllAsync(PaginationParams @params, Filter filter, string search = null);
    ValueTask<IEnumerable<WeeklyRanking>> GetAllAsync();
    ValueTask<IEnumerable<WeeklyRanking>> GetByUserIdAsync(long userId);
    ValueTask<WeeklyRanking> GetCurrentWeekRankingAsync();
    ValueTask<WeeklyRanking> GetLastWeekRankingAsync();
    ValueTask<WeeklyRanking> GetByWeekNumberAsync(int weekNumber);
} 