using Authentication_clone.Auth;
using System.ComponentModel.DataAnnotations.Schema;

namespace Authentication_clone.Models
{
    [Table("users")]
    public record class User
    {
        [Column("name")]
        public string Name { get; set; } = string.Empty;
        [Column("email")]
        public string Email { get; set; } = string.Empty;
        [Column("id")]
        public int Id { get; set; }

        [Column("role")]
        public Role Role { get; set; }

        [Column("password")]
        public string Password { get; set; } = string.Empty;
    }
}
