using NewsAggregation.Models;
using NewsAggregation.Models.DTO;

namespace NewsAggregation.Services.Interfaces
{
    public interface INotificationService
    {
        public Task<Notification> CreateNotificationAsync(CreateNotificationDto dto);
        public Task<List<UserNotificationConfigDto>> GetUserNotificationConfigAsync(int userId);
        public Task ProcessNotificationsAsync();
    }
}