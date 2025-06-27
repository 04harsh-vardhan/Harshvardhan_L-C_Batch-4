using NewsAggregation.Models;

namespace NewsAggregation.Repository.Interfaces
{
    public interface INotificationRepository
    {
        public Task<Notification> AddNotificationAsync(Notification notification);
        public Task<List<Notification>> GetNotificationsByUserIdAsync(int userId);
        public Task<List<Notification>> GetAllEnabledNotificationsAsync();
        public Task AddPendingNotificationAsync(PendingNotification pendingNotification);
        public Task UpdateNotificationLastAccessedAsync(int notificationId, DateTime lastAccessed);
        public Task<List<PendingNotification>> GetPendingNotificationsByUserIdAsync(int userId);
        public Task RemovePendingNotificationsByUserIdAsync(int userId);
        public Task<List<PendingNotification>> GetPendingNotificationsWithDetailsAsync();
    }
}