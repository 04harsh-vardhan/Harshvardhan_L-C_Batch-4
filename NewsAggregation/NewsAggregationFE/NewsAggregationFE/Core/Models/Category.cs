namespace NewsAggregationFE.Core.Models
{
    public class Category
    {
        public int Category_Id { get; set; }
        public string Category_Name { get; set; } = string.Empty;
        public bool IsHidden { get; set; } = false;
    }
}