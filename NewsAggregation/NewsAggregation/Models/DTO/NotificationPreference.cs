using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAggregation.Models.DTO
{
    public class NotificationPreference
    {
        [Column("notification_preference_id")]
        public int NotificationPreferenceId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        [Column("last_notified")]
        public DateTime LastNotified { get; set; } = DateTime.UtcNow;
    }
} 