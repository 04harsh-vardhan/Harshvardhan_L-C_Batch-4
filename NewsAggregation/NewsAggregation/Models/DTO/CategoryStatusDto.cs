namespace NewsAggregation.Models.DTO
{
    public class CategoryStatusDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsEnabled { get; set; }
    }
}