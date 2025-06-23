using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NewsAggregation.Models
{
    public class User
    {
        [Key]
        [Column("user_id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserId { get; set; }
        [Column("user_name")]
        public string Username { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("password")]
        public string Password { get; set; }
        [Column("role_id")]
        public int RoleId { get; set; }
        public Role Role { get; private set; }
        public List<Keyword> Keywords { get; private set; }
        public List<Like> Likes { get; private set; }
        public List<SavedArticle> SavedArticles { get; private set; }
    }
}
