﻿using Microsoft.Extensions.Hosting;
  using Microsoft.Extensions.Logging;
  using Microsoft.Extensions.DependencyInjection;
  using SpeakingShorts.Domain.Entities;
  using System;
  using System.Threading;
  using System.Threading.Tasks;

  namespace SpeakingShorts.Service.Services.WeeklyRankings
  {
      public class WeeklyRankingJob : BackgroundService
      {
          private readonly IServiceScopeFactory _serviceScopeFactory;
          private readonly ILogger<WeeklyRankingJob> _logger;

          public WeeklyRankingJob(IServiceScopeFactory serviceScopeFactory, ILogger<WeeklyRankingJob> logger)
          {
              _serviceScopeFactory = serviceScopeFactory ?? throw new ArgumentNullException(nameof(serviceScopeFactory));
              _logger = logger ?? throw new ArgumentNullException(nameof(logger));
          }

          protected override async Task ExecuteAsync(CancellationToken stoppingToken)
          {
              while (!stoppingToken.IsCancellationRequested)
              {
                  var now = DateTime.UtcNow; // ISystemTime dan foydalanish uchun DI'dan olish mumkin
                  if (now.DayOfWeek == DayOfWeek.Monday && now.Hour == 0 && now.Minute == 0)
                  {
                      try
                      {
                          using (var scope = _serviceScopeFactory.CreateScope())
                          {
                              var weeklyRankingService = scope.ServiceProvider.GetRequiredService<IWeeklyRankingService>();
                              await weeklyRankingService.GenerateWeeklyRankingsAsync();
                              _logger.LogInformation("Weekly rankings generated at {Time}", now);
                          }
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