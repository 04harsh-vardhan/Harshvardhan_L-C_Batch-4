namespace NewsAggregation.Models
{
    public class Notification
    {
        public int Notification_Id { get; set; }
        public bool IsEnabled { get; set; } = false;
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Keywords { get; set; }
        public DateTime LastAccessed { get; set; }
        public Category Category { get; private set; }
        public User User { get; private set; }
    }
}
