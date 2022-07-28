using Authentication_clone.Auth;
using Authentication_clone.DTOs;
using FluentValidation;

namespace Authentication_clone.Validators
{
    public class UpdateUserFormValidator: AbstractValidator<UpdateUserForm>
    {
        public UpdateUserFormValidator()
        {
            RuleFor(f => f.Name).NotEmpty().NotNull().Matches(@"[\w ]+");
            RuleFor(f => f.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(f => f.Role).NotNull().IsInEnum();
            RuleFor(f => f.Password).NotNull().NotEmpty().MinimumLength(6).MaximumLength(16).Matches(@"([a-zA-Z\d!?@#$%^&*()_;':\.])+");
        }
    }
}
