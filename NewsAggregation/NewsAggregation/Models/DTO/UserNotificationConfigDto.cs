namespace NewsAggregation.Models.DTO
{
    public class UserNotificationConfigDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsEnabled { get; set; }
        public string? Keywords { get; set; }
        public DateTime? LastAccessed { get; set; }
    }
}