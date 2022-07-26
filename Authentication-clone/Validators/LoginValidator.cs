using Authentication_clone.DTOs;
using FluentValidation;

namespace Authentication_clone.Validators
{
    public class LoginValidator: AbstractValidator<LoginForm>
    {
        public LoginValidator()
        {
            RuleFor(l => l.Password).NotNull().NotEmpty().MinimumLength(6).MaximumLength(16).Matches(@"([a-zA-Z\d!?@#$%^&*()_;':\.])+");
            RuleFor(l => l.Email).NotNull().NotEmpty().EmailAddress();
        }
    }
}
