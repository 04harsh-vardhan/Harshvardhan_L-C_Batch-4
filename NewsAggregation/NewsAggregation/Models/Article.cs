using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAggregation.Models
{
    public class Article
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("article_id")]
        public int Article_Id { get; set; }
        [Column("article_title")]
        public string Article_Title { get; set; }
        [Column("article_description")]
        public string? Article_Description { get; set; }
        [Column("article_source")]
        public string? Article_Source { get; set; }
        [Column("article_url")]
        public string? Article_Url { get; set; }
        [Column("created_at")]
        public DateTime Created_At { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        public bool IsHidden { get; set; } = false;
        public int LikesCount { get; set; }
        public int DislikesCount { get; set; }
        public List<Like> Likes { get; private set; }
        public List<SavedArticle> SavedArticles { get; private set; }
        public List<ArticleCategory> ArticleCategories { get; private set; }
        public List<PendingNotification> PendingNotifications { get; private set; }
    }
}
