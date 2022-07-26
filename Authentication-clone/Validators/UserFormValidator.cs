using Authentication_clone.DTOs;
using FluentValidation;

namespace Authentication_clone.Validators
{
    public class UserFormValidator: AbstractValidator<UserForm>
    {
        public UserFormValidator()
        {
            RuleFor(u => u.Name).NotEmpty().NotNull().Matches(@"[\w ]+");
            RuleFor(u => u.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(u => u.Password).NotNull().NotEmpty().MinimumLength(6).MaximumLength(16).Matches(@"([a-zA-Z\d!?@#$%^&*()_;':\.])+");
        }
    }
}
