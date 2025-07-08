namespace NewsAggregationFE.Core.Models
{
    public class CategoryStatusResponse
    {
        public bool Success { get; set; }
        public List<CategoryStatusDto> Data { get; set; } = new List<CategoryStatusDto>();
        public string Message { get; set; } = string.Empty;
    }
}