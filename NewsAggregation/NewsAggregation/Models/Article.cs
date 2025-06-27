namespace NewsAggregation.Models
{
    public class Article
    {
        public int Article_Id { get; set; }
        public string Article_Title { get; set; }
        public string? Article_Description { get; set; }
        public string? Article_Source { get; set; }
        public string? Article_Url { get; set; }
        public DateTime Created_At { get; set; } = DateTime.UtcNow;
        public bool IsHidden { get; set; } = false;
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public int ReportCount { get; set; } = 0;
        public List<Like> Likes { get; private set; }
        public List<SavedArticle> SavedArticles { get; private set; }
        public List<ArticleCategory> ArticleCategories { get; private set; }
        public List<PendingNotification> PendingNotifications { get; private set; }
    }
}
