namespace NewsAggregation.Models
{
    public class PendingNotification
    {
        public int PendingNotificationId { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public User User { get; set; }
        public Article Article { get; set; }

    }
}
