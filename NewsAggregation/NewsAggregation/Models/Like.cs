namespace NewsAggregation.Models
{
    public enum LikeStatus
    {
        Like = 1,
        DisLike = 2
    }
    public class Like
    {
        public int LikeId { get; set; }
        public int UserId { get; set; }
        public int ArticleId { get; set; }
        public LikeStatus Status { get; set; }
        public User User { get; private set; }
        public Article Article { get; private set; }
    }
}
