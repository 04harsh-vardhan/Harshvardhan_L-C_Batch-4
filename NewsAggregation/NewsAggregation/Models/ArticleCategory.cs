namespace NewsAggregation.Models
{
    public class ArticleCategory
    {
        public int ArticleId { get; set; }
        public int CategoryId { get; set; }
        public int ArticleCategoryId { get; set; }
        public Article Article { get; set; }
        public Category Category { get; private set; }
    }
}
