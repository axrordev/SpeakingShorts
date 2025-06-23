using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SpeakingShorts.Service.Services.Processing
{
    public class VideoProcessingHostedService : BackgroundService
    {
        private readonly ILogger<VideoProcessingHostedService> _logger;
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceProvider _serviceProvider;

        public VideoProcessingHostedService(
            ILogger<VideoProcessingHostedService> logger,
            IBackgroundTaskQueue taskQueue,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _taskQueue = taskQueue;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Video Processing Hosted Service is running.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        await workItem(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing {WorkItem}.", nameof(workItem));
                }
            }

            _logger.LogInformation("Video Processing Hosted Service is stopping.");
        }
    }
} 