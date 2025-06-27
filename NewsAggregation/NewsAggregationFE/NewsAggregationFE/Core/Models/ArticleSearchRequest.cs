namespace NewsAggregationFE.Core.Models
{
    public class ArticleSearchRequest
    {
        public string Keyword { get; set; } = string.Empty;
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    public class ArticleResponse
    {
        public bool Success { get; set; }
        public List<Article> Data { get; set; } = new();
        public string? Message { get; set; }
    }
}