namespace NewsAggregationFE.Core.Models
{
    public class UpdateCategoryStatusDto
    {
        public int CategoryId { get; set; }
        public bool IsEnabled { get; set; }
    }
}