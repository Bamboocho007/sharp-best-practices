using Authentication_clone.Auth;

namespace Authentication_clone.DTOs
{
    public record class UpdateUserForm(string Name, string Email, Role Role, string Password) {}
}
