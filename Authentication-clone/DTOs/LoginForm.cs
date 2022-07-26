namespace Authentication_clone.DTOs
{
    public class LoginForm
    {
        public string Email { get; init; }
        public string Password { get; init; }

        public LoginForm(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
