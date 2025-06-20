using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAggregation.Models
{
    public class ExternalServer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("server_id")]
        public int Server_ID { get; set; }
        [Column("server_name")]
        public string Server_Name { get; set; }
        [Column("server_status")]
        public bool Server_Status { get; set; }
        [Column("server_url")]
        public string Server_URL { get; set; }
        [Column("server_api_key")]
        public string Server_API_KEY { get; set; }
        [Column("last_accessed")]
        public DateTime Last_accessed { get; set; }

    }
}
