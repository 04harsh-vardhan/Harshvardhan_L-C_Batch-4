using Microsoft.EntityFrameworkCore;
using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
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

        public async Task<bool> AddNotificationPreference(NotificationPreference preference)
        {
            await _dbContext.NotificationPreferences.AddAsync(preference);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeactivateUserPreferences(int userId)
        {
            var preferences = await _dbContext.NotificationPreferences
                .Where(p => p.UserId == userId)
                .ToListAsync();

            foreach (var preference in preferences)
            {
                preference.IsActive = false;
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<NotificationPreference>> GetActivePreferencesByCategoryId(int categoryId)
        {
            return await _dbContext.NotificationPreferences
                .Where(p => p.CategoryId == categoryId && p.IsActive)
                .ToListAsync();
        }

        public async Task<bool> UpdateLastNotified(int preferenceId)
        {
            var preference = await _dbContext.NotificationPreferences
                .FirstOrDefaultAsync(p => p.NotificationPreferenceId == preferenceId);

            if (preference != null)
            {
                preference.LastNotified = DateTime.UtcNow;
                await _dbContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
} 