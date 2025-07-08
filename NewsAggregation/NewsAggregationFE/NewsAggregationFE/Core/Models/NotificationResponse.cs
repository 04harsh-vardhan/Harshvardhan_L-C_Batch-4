namespace NewsAggregationFE.Core.Models
{
    public class NotificationResponse
    {
        public bool Success { get; set; }
        public List<Article> Data { get; set; } = new List<Article>();
        public string Message { get; set; } = string.Empty;
    }
}