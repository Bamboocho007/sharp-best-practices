using Authentication_clone.Auth;
using Authentication_clone.Db;
using Authentication_clone.DTOs;
using Authentication_clone.Models;
using Authentication_clone.Validators;

namespace Authentication_clone.ModelServices
{
    public static class UserService
    {
        public static async Task<User?> GetInfo(string tokenString, IConfiguration config)
        {
            var idFromClaims = JwtHelper.GetJWTTokenClaim(tokenString, "Id");
            int id = int.TryParse(idFromClaims, out id) ? id : 0;
            return await new UsersRepo(config).GetById(id);
        }

        public static async Task<ResponseData<User?>> Create(UserForm form, IConfiguration config)
        {
            UserFormValidator validator = new UserFormValidator();

            var validated = validator.Validate(form);
            var repo = new UsersRepo(config);

            if (await repo.GetByEmail(form.Email) != null)
            {
                return new ResponseData<User?>("User with same email already exists!");
            }

            if (validated.IsValid)
            {
                User formUser = new() { Name = form.Name, Email = form.Email, Password = PasswordHelper.HashPassword(form.Password), Role = Role.Contributor};
                var user = await repo.Add(formUser);
                return new ResponseData<User?>(user);
            }

            var errors = new List<ErrorsDescription>();

            foreach (var error in validated.Errors)
            {
                errors.Add(new ErrorsDescription { Message = error.ErrorMessage, PropertyName = error.PropertyName });
            }
            return new ResponseData<User?>("Form data is not valid!", errors);
        }
    }
}
