namespace NewsAggregation.Models.DTO
{
    public class CreateNotificationDto
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public string Keywords { get; set; }
        public bool IsEnabled { get; set; }
    }
}
