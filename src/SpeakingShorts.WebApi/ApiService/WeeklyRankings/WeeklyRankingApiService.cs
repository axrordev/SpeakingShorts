using AutoMapper;
using SpeakingShorts.Service.Services.WeeklyRankings;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Configurations;
using SpeakingShorts.WebApi.Models.WeeklyRankings;

namespace SpeakingShorts.WebApi.ApiService.WeeklyRankings
{
    public class WeeklyRankingApiService(IWeeklyRankingService weeklyRankingService, IMapper mapper) : IWeeklyRankingApiService
    {
        private readonly IWeeklyRankingService _weeklyRankingService = weeklyRankingService ?? throw new ArgumentNullException(nameof(weeklyRankingService));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        public async ValueTask<bool> DeleteAsync(long id)
        {
            return await _weeklyRankingService.DeleteAsync(id);
        }

        public async ValueTask<WeeklyRankingViewModel> GetAsync(long id)
        {
            var weeklyRanking = await _weeklyRankingService.GetAsync(id);
            return _mapper.Map<WeeklyRankingViewModel>(weeklyRanking);
        }

        public async ValueTask<IEnumerable<WeeklyRankingViewModel>> GetAllAsync(PaginationParams @params, Filter filter, string search = null)
        {
            var weeklyRankings = await _weeklyRankingService.GetAllAsync(@params, filter, search);
            return _mapper.Map<IEnumerable<WeeklyRankingViewModel>>(weeklyRankings);
        }

        public async ValueTask<IEnumerable<WeeklyRankingViewModel>> GetAllAsync()
        {
            var weeklyRankings = await _weeklyRankingService.GetAllAsync();
            return _mapper.Map<IEnumerable<WeeklyRankingViewModel>>(weeklyRankings);
        }

        public async ValueTask<IEnumerable<WeeklyRankingViewModel>> GetByUserIdAsync(long userId)
        {
            var weeklyRankings = await _weeklyRankingService.GetByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<WeeklyRankingViewModel>>(weeklyRankings);
        }

        public async ValueTask<WeeklyRankingViewModel> GetCurrentWeekRankingAsync()
        {
            var weeklyRanking = await _weeklyRankingService.GetCurrentWeekRankingAsync();
            return _mapper.Map<WeeklyRankingViewModel>(weeklyRanking);
        }

        public async ValueTask<WeeklyRankingViewModel> GetLastWeekRankingAsync()
        {
            var weeklyRanking = await _weeklyRankingService.GetLastWeekRankingAsync();
            return _mapper.Map<WeeklyRankingViewModel>(weeklyRanking);
        }

        public async ValueTask<WeeklyRankingViewModel> GetByWeekNumberAsync(int weekNumber)
        {
            var weeklyRanking = await _weeklyRankingService.GetByWeekNumberAsync(weekNumber);
            return _mapper.Map<WeeklyRankingViewModel>(weeklyRanking);
        }
    }
}