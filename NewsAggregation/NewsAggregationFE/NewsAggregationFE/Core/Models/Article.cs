namespace NewsAggregationFE.Core.Models
{
    public class Article
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Url { get; set; }
        public string Category { get; set; }
        public DateTime PublishedAt { get; set; }
        public int Likes { get; set; } = 0;
        public int Dislikes { get; set; } = 0;
    }
}