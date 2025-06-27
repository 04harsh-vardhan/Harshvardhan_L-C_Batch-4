namespace NewsAggregation.Models.DTO
{
    public class NotificationPreference
    {
        public int NotificationPreferenceId { get; set; }

        public int UserId { get; set; }

        public int CategoryId { get; set; }

        public string Email { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime LastNotified { get; set; } = DateTime.UtcNow;
    }
} 