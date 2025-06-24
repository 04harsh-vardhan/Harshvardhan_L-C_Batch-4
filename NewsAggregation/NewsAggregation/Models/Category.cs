namespace NewsAggregation.Models
{
    public class Category
    {
        public int Category_Id { get; set; }
        public string Category_Name { get; set; }
        public List<ArticleCategory> ArticleCategories { get; private set; }
        public List<Notification> Notifications { get; private set; }
        public List<PendingNotification> PendingNotifications { get; private set; }
    }
}
