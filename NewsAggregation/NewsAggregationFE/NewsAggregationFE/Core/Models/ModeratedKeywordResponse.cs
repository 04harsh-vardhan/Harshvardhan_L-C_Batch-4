namespace NewsAggregationFE.Core.Models
{
    public class ModeratedKeywordResponse
    {
        public bool Success { get; set; }
        public List<ModeratedKeywordDto> Data { get; set; } = new List<ModeratedKeywordDto>();
        public string Message { get; set; } = string.Empty;
    }
}