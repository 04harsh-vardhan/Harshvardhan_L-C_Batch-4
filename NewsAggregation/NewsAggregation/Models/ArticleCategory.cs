using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAggregation.Models
{
    public class ArticleCategory
    {
        [Column("article_id")]
        public int ArticleId { get; set; }
        [Column("category_id")]
        public int CategoryId { get; set; }
        [Column("Article_category_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ArticleCategoryId { get; set; }
    }
}
