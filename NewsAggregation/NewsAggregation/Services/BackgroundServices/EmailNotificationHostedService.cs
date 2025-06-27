using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services.BackgroundServices
{
    public class EmailNotificationHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmailNotificationHostedService> _logger;
        private readonly TimeSpan _emailInterval = TimeSpan.FromHours(6);

        public EmailNotificationHostedService(
            IServiceProvider serviceProvider,
            ILogger<EmailNotificationHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Email notification background task running at {Time}", DateTimeOffset.Now);
                
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();
                    
                    await notificationService.ProcessEmailNotificationsAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in email notification background task: {Message}", ex.Message);
                }
                
                await Task.Delay(_emailInterval, stoppingToken);
            }
        }
    }
}