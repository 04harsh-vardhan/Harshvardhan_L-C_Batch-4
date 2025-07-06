namespace NewsAggregationFE.Core.Models
{
    public class UserNotificationConfigDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
        public string? Keywords { get; set; }
        public DateTime? LastAccessed { get; set; }
    }
}