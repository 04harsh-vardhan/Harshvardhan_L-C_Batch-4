namespace NewsAggregation.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; private set; }
        public List<Like> Likes { get; private set; }
        public List<SavedArticle> SavedArticles { get; private set; }
        public List<Notification> Notifications { get; private set; }
        public List<PendingNotification> PendingNotifications { get; private set; }
    }
}
