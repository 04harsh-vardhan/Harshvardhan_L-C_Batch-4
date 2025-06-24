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
                ArticleService? articleService = _serviceProvider.GetService<ArticleService>();
                if (articleService != null)
                {
                    await articleService.SyncNews();
                }
                await Task.Delay(_syncInterval, stoppingToken);
            }
        }
    }
}