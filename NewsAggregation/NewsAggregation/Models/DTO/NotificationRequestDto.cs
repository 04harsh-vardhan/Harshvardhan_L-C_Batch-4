namespace NewsAggregation.Models.DTO
{
    public class NotificationRequestDto
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public List<string> Categories { get; set; }
    }
} 