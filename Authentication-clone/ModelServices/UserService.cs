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
            UserFormValidator validator = new ();
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

            return new ResponseData<User?>("Form data is not valid!", validated.Errors);
        }

        public static async Task<ResponseData<User?>> Update(UpdateUserForm form, int? userId, IConfiguration config)
        {
            if (userId == null)
            {
                return new ResponseData<User?>("User id is required!");
            }

            UpdateUserFormValidator validator = new();
            var validated = validator.Validate(form);
            var repo = new UsersRepo(config);
            var currentUser = await repo.GetById((int)userId);

            if (currentUser != null)
            {
                if(validated.IsValid)
                {
                    var updatedUser = await repo.Update(new User { Id = currentUser.Id, Name = form.Name, Email = form.Email, Role = form.Role, Password = PasswordHelper.HashPassword(form.Password) });
                    return new ResponseData<User?>(updatedUser);
                }

                return new ResponseData<User?>("Form data is not valid!", validated.Errors);
            }

            return new ResponseData<User?>("User no exists!");
        }

        public static async Task<ResponseData<User?>> Delete(int? id, IConfiguration config)
        {
            if (id == null)
            {
                return new ResponseData<User?>("User id is required!");
            }

            var repo = new UsersRepo(config);
            return new ResponseData<User?>(await repo.Delete((int)id));
        }
    }
}
