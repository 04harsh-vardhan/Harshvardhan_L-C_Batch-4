using NewsAggregation.Models.DTO;

namespace NewsAggregation.Services.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SetNotificationPreferences(NotificationRequestDto request);
        Task<bool> UpdateNotificationPreferences(NotificationRequestDto request);
        Task<bool> DisableNotifications(int userId);
        Task ProcessNewArticleNotifications(int categoryId);
    }
} 