using Newtonsoft.Json;

namespace NewsAggregationFE.Core.Models
{
    public class ExternalServerResponse
    {
        public List<Data> Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("server_Name")]
        public string ServerName { get; set; }
        [JsonProperty("server_ID")]
        public int ServerID { get; set; }
        [JsonProperty("server_Status")]
        public bool ServerStatus { get; set; }
        [JsonProperty("server_UR")]
        public string ServerURL { get; set; }
        [JsonProperty("server_API_KEY")]
        public string ServerAPIKEY { get; set; }
        [JsonProperty("last_accessed")]
        public DateTime LastAccessed { get; set; }
    }
}
