namespace NewsAggregation.Models.DTO
{
    public class ExternalServerDto
    {
        public string ServerName { get; set; }
        public bool ServerStatus { get; set; }
        public string ServerURL { get; set; }
        public string ServerAPIKEY { get; set; }
        public DateTime LastAccessed { get; set; } = DateTime.UtcNow;

    }
}
