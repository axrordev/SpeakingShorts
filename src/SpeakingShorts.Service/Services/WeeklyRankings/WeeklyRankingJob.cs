using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.Service.Services.Infrastructure.Utilities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpeakingShorts.Service.Services.WeeklyRankings
{
    public class WeeklyRankingJob : BackgroundService
    {
        private readonly IWeeklyRankingService _weeklyRankingService;
        private readonly ISystemTime _systemTime;
        private readonly ILogger<WeeklyRankingJob> _logger;

        public WeeklyRankingJob(IWeeklyRankingService weeklyRankingService, ISystemTime systemTime, ILogger<WeeklyRankingJob> logger)
        {
            _weeklyRankingService = weeklyRankingService ?? throw new ArgumentNullException(nameof(weeklyRankingService));
            _systemTime = systemTime ?? throw new ArgumentNullException(nameof(systemTime));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = _systemTime.UtcNow;
                if (now.DayOfWeek == DayOfWeek.Monday && now.Hour == 0 && now.Minute == 0)
                {
                    try
                    {
                        await _weeklyRankingService.GenerateWeeklyRankingsAsync();
                        _logger.LogInformation("Weekly rankings generated at {Time}", now);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error generating weekly rankings");
                    }
                }
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}