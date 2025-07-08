namespace NewsAggregationFE.Core.Models
{
    public class CreateNotificationDto
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Keywords { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }
}