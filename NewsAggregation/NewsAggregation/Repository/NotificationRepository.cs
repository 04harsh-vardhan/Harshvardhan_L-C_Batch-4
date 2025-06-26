using Microsoft.EntityFrameworkCore;
using NewsAggregation.Models;
using NewsAggregation.Repository.Interfaces;

namespace NewsAggregation.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NewsAggDBContext _dbContext;

        public NotificationRepository(NewsAggDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Notification> AddNotificationAsync(Notification notification)
        {
            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();
            return notification;
        }
        
        public async Task<List<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _dbContext.Notifications
                .Include(n => n.Category)
                .Where(n => n.UserId == userId)
                .ToListAsync();
        }
        
        public async Task<List<Notification>> GetAllEnabledNotificationsAsync()
        {
            return await _dbContext.Notifications
                .Include(n => n.Category)
                .Include(n => n.User)
                .Where(n => n.IsEnabled)
                .ToListAsync();
        }
        
        public async Task AddPendingNotificationAsync(PendingNotification pendingNotification)
        {
            _dbContext.PendingNotifications.Add(pendingNotification);
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task UpdateNotificationLastAccessedAsync(int notificationId, DateTime lastAccessed)
        {
            var notification = await _dbContext.Notifications.FindAsync(notificationId);
            if (notification != null)
            {
                notification.LastAccessed = lastAccessed;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}