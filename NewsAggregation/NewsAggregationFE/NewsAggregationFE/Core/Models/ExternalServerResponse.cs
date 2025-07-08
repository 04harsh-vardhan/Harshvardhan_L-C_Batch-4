using Newtonsoft.Json;

namespace NewsAggregationFE.Core.Models
{
    public class ExternalServer
    {
        [JsonProperty("server_ID")]
        public int Server_ID { get; set; }
        
        [JsonProperty("server_Name")]
        public string Server_Name { get; set; } = string.Empty;
        
        [JsonProperty("server_Status")]
        public bool Server_Status { get; set; }
        
        [JsonProperty("server_URL")]
        public string Server_URL { get; set; } = string.Empty;
        
        [JsonProperty("server_API_KEY")]
        public string Server_API_KEY { get; set; } = string.Empty;
        
        [JsonProperty("last_accessed")]
        public DateTime Last_accessed { get; set; }
    }
}
