namespace NewsAggregationFE.Core.Models
{
    public class ServerDetailRequestBody
    {
        public int ServerId { get; set; }
        public string ServerApi { get; set; }
        public bool ServerStatus { get; set; }
    }
}
