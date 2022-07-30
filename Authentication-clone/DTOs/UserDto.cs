using Authentication_clone.Auth;

namespace Authentication_clone.DTOs
{
    public class UserDto
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public int Id { get; set; }

        public Role Role { get; set; }
    }
}
