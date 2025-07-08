namespace NewsAggregation.Models.DTO
{
    public class ArticleSearchRequest
    {
        public string Keyword { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
