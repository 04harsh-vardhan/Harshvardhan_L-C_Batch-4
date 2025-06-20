using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAggregation.Models
{
    public class Category
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("category_id")]
        public int Category_Id { get; set; }
        [Column("category_name")]
        public string Category_Name { get; set; }
    }
}
