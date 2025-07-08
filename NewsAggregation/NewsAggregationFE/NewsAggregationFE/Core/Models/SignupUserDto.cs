namespace NewsAggregationFE.Core.Models
{
    public class SignupUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int RoleId { get; set; } = 2; // Default to user role
    }
}