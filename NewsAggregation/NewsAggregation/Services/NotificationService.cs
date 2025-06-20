using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(
            IEmailService emailService,
            IArticleRepository articleRepository,
            ICategoryRepository categoryRepository,
            INotificationRepository notificationRepository)
        {
            _emailService = emailService;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<bool> SetNotificationPreferences(NotificationRequestDto request)
        {
            foreach (var categoryName in request.Categories)
            {
                var categoryId = await _categoryRepository.GetCategoryIdByName(categoryName);
                if (categoryId == 0)
                {
                    continue;
                }

                var preference = new NotificationPreference
                {
                    UserId = request.UserId,
                    CategoryId = categoryId,
                    Email = request.Email,
                    IsActive = true,
                    LastNotified = DateTime.UtcNow
                };

                await _notificationRepository.AddNotificationPreference(preference);
            }

            return true;
        }

        public async Task<bool> UpdateNotificationPreferences(NotificationRequestDto request)
        {
            await _notificationRepository.DeactivateUserPreferences(request.UserId);
            return await SetNotificationPreferences(request);
        }

        public async Task<bool> DisableNotifications(int userId)
        {
            return await _notificationRepository.DeactivateUserPreferences(userId);
        }

        public async Task ProcessNewArticleNotifications(int categoryId)
        {
            var preferences = await _notificationRepository.GetActivePreferencesByCategoryId(categoryId);
            if (!preferences.Any())
            {
                return;
            }

            foreach (var preference in preferences)
            {
                var newArticles = await _articleRepository.GetNewArticlesSince(categoryId, preference.LastNotified);
                
                foreach (var article in newArticles)
                {
                    await _emailService.SendArticleNotificationAsync(preference.Email, article);
                }

                if (newArticles.Any())
                {
                    await _notificationRepository.UpdateLastNotified(preference.NotificationPreferenceId);
                }
            }
        }
    }
} 