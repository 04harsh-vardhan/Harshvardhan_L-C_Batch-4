namespace NewsAggregationFE.Core.Models
{
    public class NotificationConfigResponse
    {
        public bool Success { get; set; }
        public List<UserNotificationConfigDto> Data { get; set; } = new List<UserNotificationConfigDto>();
        public string Message { get; set; } = string.Empty;
    }
}