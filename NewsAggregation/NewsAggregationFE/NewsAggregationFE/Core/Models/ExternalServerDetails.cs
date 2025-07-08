namespace NewsAggregationFE.Core.Models
{
    public class ExternalServerList
    {
        public string ApiName { get; set; }
        public string Status { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}
