namespace NewsAggregation.Models
{
    public class ExternalServer
    {
        public int Server_ID { get; set; }
        public string Server_Name { get; set; }
        public bool Server_Status { get; set; }
        public string Server_URL { get; set; }
        public string Server_API_KEY { get; set; }
        public DateTime Last_accessed { get; set; }

    }
}
