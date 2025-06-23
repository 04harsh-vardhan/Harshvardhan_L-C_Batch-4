using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAggregation.Models
{
    public class Role
    {
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("role")]
        public string UserRole { get; set; }
        public List<User> Users;
    }
}
