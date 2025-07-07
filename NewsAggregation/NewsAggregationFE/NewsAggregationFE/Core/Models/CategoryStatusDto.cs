namespace NewsAggregationFE.Core.Models
{
    public class CategoryStatusDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsEnabled { get; set; }
    }
}