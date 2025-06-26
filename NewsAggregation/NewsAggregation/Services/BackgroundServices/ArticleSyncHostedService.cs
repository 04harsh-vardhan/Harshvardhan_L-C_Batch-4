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
                _logger.LogInformation("Sync News Background task running {time}", DateTimeOffset.Now);
                
                using var scope = _serviceProvider.CreateScope();
                var newsApiAdapters = scope.ServiceProvider.GetServices<INewsApiAdapter>().ToList();
                
                await SyncNewsWithFallback(newsApiAdapters);
                await Task.Delay(_syncInterval, stoppingToken);
            }
        }

        private async Task SyncNewsWithFallback(List<INewsApiAdapter> adapters)
        {
            foreach (var adapter in adapters)
            {
                try
                {
                    _logger.LogInformation("Attempting to sync news using {ApiName}", adapter.ApiName);
                    var articles = await adapter.FetchAndMapArticlesAsync();
                    _logger.LogInformation("Successfully synced {Count} articles from {ApiName}", articles.Count, adapter.ApiName);
                    return; // Success, exit the loop
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to sync news from {ApiName}: {Message}", adapter.ApiName, ex.Message);
                    // Continue to next adapter
                }
            }
            
            _logger.LogError("All news API adapters failed to sync news");
        }
    }
}