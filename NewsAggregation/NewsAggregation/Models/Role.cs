namespace NewsAggregation.Models
{
    public class Role
    {
        public int RoleId { get; set; }
        public string UserRole { get; set; }
        public List<User> Users;
    }
}
