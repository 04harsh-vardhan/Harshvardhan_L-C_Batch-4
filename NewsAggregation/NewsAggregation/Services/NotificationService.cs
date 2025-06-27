using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository notificationRepository,
            ICategoryRepository categoryRepository,
            IArticleRepository articleRepository,
            IEmailService emailService,
            ILogger<NotificationService> logger)
        {
            _notificationRepository = notificationRepository;
            _categoryRepository = categoryRepository;
            _articleRepository = articleRepository;
            _emailService = emailService;
            _logger = logger;
        }
        public async Task<Notification> CreateNotificationAsync(CreateNotificationDto dto)
        {
            var notification = new Notification
            {
                UserId = dto.UserId,
                CategoryId = dto.CategoryId,
                Keywords = dto.Keywords,
                IsEnabled = dto.IsEnabled,
                LastAccessed = DateTime.UtcNow
            };

            return await _notificationRepository.AddNotificationAsync(notification);
        }

        public async Task<List<UserNotificationConfigDto>> GetUserNotificationConfigAsync(int userId)
        {
            var allCategories = await _categoryRepository.GetAllCategories();
            var userNotifications = await _notificationRepository.GetNotificationsByUserIdAsync(userId);
            
            var notificationDict = userNotifications.ToDictionary(n => n.CategoryId, n => n);
            
            var result = allCategories.Select(category => new UserNotificationConfigDto
            {
                CategoryId = category.Category_Id,
                CategoryName = category.Category_Name,
                IsEnabled = notificationDict.ContainsKey(category.Category_Id) ? notificationDict[category.Category_Id].IsEnabled : false,
                Keywords = notificationDict.ContainsKey(category.Category_Id) ? notificationDict[category.Category_Id].Keywords : null,
                LastAccessed = notificationDict.ContainsKey(category.Category_Id) ? notificationDict[category.Category_Id].LastAccessed : null
            }).ToList();
            
            return result;
        }

        public async Task ProcessNotificationsAsync()
        {
            try
            {
                _logger.LogInformation("Starting notification processing at {Time}", DateTime.UtcNow);

                // Get all enabled notifications
                var enabledNotifications = await _notificationRepository.GetAllEnabledNotificationsAsync();
                _logger.LogInformation("Found {Count} enabled notifications to process", enabledNotifications.Count);

                foreach (var notification in enabledNotifications)
                {
                    await ProcessUserNotificationAsync(notification);
                }

                _logger.LogInformation("Completed notification processing at {Time}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during notification processing: {Message}", ex.Message);
                throw;
            }
        }

        private async Task ProcessUserNotificationAsync(Notification notification)
        {
            try
            {
                _logger.LogDebug("Processing notification for User {UserId}, Category {CategoryId}", 
                    notification.UserId, notification.CategoryId);

                // Get articles added after the last accessed date for the specific category
                var newArticles = await _articleRepository.GetNewArticlesSince(
                    notification.CategoryId, 
                    notification.LastAccessed);

                if (!newArticles.Any())
                {
                    _logger.LogDebug("No new articles found for User {UserId}, Category {CategoryId}", 
                        notification.UserId, notification.CategoryId);
                    return;
                }

                // Filter articles by keywords if specified
                var matchingArticles = FilterArticlesByKeywords(newArticles, notification.Keywords);

                if (!matchingArticles.Any())
                {
                    _logger.LogDebug("No articles matched keywords for User {UserId}, Category {CategoryId}", 
                        notification.UserId, notification.CategoryId);
                    return;
                }

                // Add pending notifications for matching articles
                foreach (var article in matchingArticles)
                {
                    var pendingNotification = new PendingNotification
                    {
                        UserId = notification.UserId,
                        ArticleId = article.Article_Id
                    };

                    await _notificationRepository.AddPendingNotificationAsync(pendingNotification);
                }

                // Update the notification's last accessed date
                await _notificationRepository.UpdateNotificationLastAccessedAsync(
                    notification.Notification_Id, 
                    DateTime.UtcNow);

                _logger.LogInformation("Added {Count} pending notifications for User {UserId}, Category {CategoryId}", 
                    matchingArticles.Count, notification.UserId, notification.CategoryId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing notification for User {UserId}, Category {CategoryId}: {Message}", 
                    notification.UserId, notification.CategoryId, ex.Message);
            }
        }

        private List<Article> FilterArticlesByKeywords(List<Article> articles, string? keywords)
        {
            if (string.IsNullOrWhiteSpace(keywords))
            {
                return articles;
            }

            var keywordList = keywords.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(k => k.Trim().ToLower())
                .Where(k => !string.IsNullOrEmpty(k))
                .ToList();

            if (!keywordList.Any())
            {
                return articles;
            }

            return articles.Where(article =>
            {
                var searchText = $"{article.Article_Title} {article.Article_Description}".ToLower();
                return keywordList.Any(keyword => searchText.Contains(keyword));
            }).ToList();
        }

        public async Task ProcessEmailNotificationsAsync()
        {
            try
            {
                _logger.LogInformation("Starting email notification processing at {Time}", DateTime.UtcNow);

                var pendingNotifications = await _notificationRepository.GetPendingNotificationsWithDetailsAsync();
                
                if (!pendingNotifications.Any())
                {
                    _logger.LogInformation("No pending notifications found for email processing");
                    return;
                }

                var groupedByUser = pendingNotifications
                    .GroupBy(pn => new { pn.UserId, pn.User.Email, pn.User.Username })
                    .ToList();

                _logger.LogInformation("Processing email notifications for {UserCount} users with total {NotificationCount} pending notifications", 
                    groupedByUser.Count, pendingNotifications.Count);

                foreach (var userGroup in groupedByUser)
                {
                    await ProcessUserEmailNotificationsAsync(userGroup.Key.Email, userGroup.Key.Username, userGroup.ToList());
                }

                _logger.LogInformation("Completed email notification processing at {Time}", DateTime.UtcNow);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during email notification processing: {Message}", ex.Message);
                throw;
            }
        }

        private async Task ProcessUserEmailNotificationsAsync(string email, string username, List<PendingNotification> pendingNotifications)
        {
            try
            {
                _logger.LogInformation("Processing email notifications for user: {Email} with {Count} pending notifications", 
                    email, pendingNotifications.Count);

                var articles = pendingNotifications.Select(pn => pn.Article).ToList();
                
                await _emailService.SendGroupedArticleNotificationsAsync(email, username, articles);
                
                _logger.LogInformation("Email notification sent successfully to {Email} for {Count} articles", 
                    email, articles.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to process email notifications for user {Email}: {Message}", 
                    email, ex.Message);
            }
        }

        public async Task<List<Article>> ViewNotificationsAsync(int userId)
        {
            try
            {
                _logger.LogInformation("ViewNotifications request for UserId: {UserId}", userId);

                var pendingNotifications = await _notificationRepository.GetPendingNotificationsByUserIdAsync(userId);
                
                if (!pendingNotifications.Any())
                {
                    _logger.LogInformation("No pending notifications found for UserId: {UserId}", userId);
                    return new List<Article>();
                }

                var articles = pendingNotifications
                    .Where(pn => pn.Article != null)
                    .Select(pn => pn.Article)
                    .ToList();

                await _notificationRepository.RemovePendingNotificationsByUserIdAsync(userId);

                _logger.LogInformation("ViewNotifications completed - Returned {Count} articles and cleared pending notifications for UserId: {UserId}", 
                    articles.Count, userId);

                return articles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ViewNotifications failed for UserId {UserId}: {Message}", userId, ex.Message);
                throw;
            }
        }
    }
}