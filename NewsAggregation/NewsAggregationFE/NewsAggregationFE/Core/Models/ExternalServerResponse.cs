namespace NewsAggregationFE.Core.Models
{
    public class ExternalServerResponse
    {
        public List<Data> Data { get; set; }
    }

    public class Data
    {
        public string ServerName { get; set; }
        public bool ServerStatus { get; set; }
        public string ServerURL { get; set; }
        public string ServerAPIKEY { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}
