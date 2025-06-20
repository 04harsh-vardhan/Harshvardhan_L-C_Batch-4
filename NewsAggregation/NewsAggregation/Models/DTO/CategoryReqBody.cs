using Newtonsoft.Json;

namespace NewsAggregation.Models.DTO
{
    public class CategoryReqBody
    {
        [JsonProperty("categoryName")]
        public string CategoryName { get; set; }
    }
}
