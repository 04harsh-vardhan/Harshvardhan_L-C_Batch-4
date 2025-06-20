using NewsAggregation.Models.DTO;

namespace NewsAggregation.Repository.Interfaces
{
    public interface INotificationRepository
    {
        Task<bool> AddNotificationPreference(NotificationPreference preference);
        Task<bool> DeactivateUserPreferences(int userId);
        Task<List<NotificationPreference>> GetActivePreferencesByCategoryId(int categoryId);
        Task<bool> UpdateLastNotified(int preferenceId);
    }
} 