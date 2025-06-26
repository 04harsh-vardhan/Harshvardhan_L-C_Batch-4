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

        public NotificationService(
            INotificationRepository notificationRepository,
            ICategoryRepository categoryRepository)
        {
            _notificationRepository = notificationRepository;
            _categoryRepository = categoryRepository;
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
    }
}