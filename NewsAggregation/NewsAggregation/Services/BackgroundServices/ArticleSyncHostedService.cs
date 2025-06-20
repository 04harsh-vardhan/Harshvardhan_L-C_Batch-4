using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services.BackgroundServices
{
    public class ArticleSyncHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ArticleSyncHostedService> _logger;
        private readonly TimeSpan _syncInterval = TimeSpan.FromHours(6);

        public ArticleSyncHostedService(
            IServiceProvider serviceProvider,
            ILogger<ArticleSyncHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var articleService = scope.ServiceProvider.GetRequiredService<IArticleService>();
                        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                        _logger.LogInformation("Starting article sync at: {time}", DateTimeOffset.Now);
                        await articleService.SyncNews();
                        _logger.LogInformation("Article sync completed at: {time}", DateTimeOffset.Now);

                        var categories = await scope.ServiceProvider.GetRequiredService<ICategoryRepository>().GetAllCategories();
                        foreach (var category in categories)
                        {
                            await notificationService.ProcessNewArticleNotifications(category.Category_Id);
                        }
                        _logger.LogInformation("Notifications processed at: {time}", DateTimeOffset.Now);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while syncing articles at: {time}", DateTimeOffset.Now);
                }

                await Task.Delay(_syncInterval, stoppingToken);
            }
        }
    }
} 