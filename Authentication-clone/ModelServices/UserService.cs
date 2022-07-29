using Authentication_clone.Auth;
using Authentication_clone.Db;
using Authentication_clone.DTOs;
using Authentication_clone.Models;
using Authentication_clone.Validators;

namespace Authentication_clone.ModelServices
{
    public class UserService
    {
        private readonly UsersRepo _usersRepo;
        public UserService(UsersRepo usersRepo)
        {
            _usersRepo = usersRepo;
        }

        public async Task<User?> GetInfo(string tokenString)
        {
            var idFromClaims = JwtHelper.GetJWTTokenClaim(tokenString, "Id");
            int id = int.TryParse(idFromClaims, out id) ? id : 0;
            return await _usersRepo.GetById(id);
        }

        public async Task<ResponseData<User?>> Create(UserForm form)
        {
            UserFormValidator validator = new ();
            var validated = validator.Validate(form);
            var repo = _usersRepo;

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

        public async Task<ResponseData<User?>> Update(UpdateUserForm form, int? userId)
        {
            if (userId == null)
            {
                return new ResponseData<User?>("User id is required!");
            }

            UpdateUserFormValidator validator = new();
            var validated = validator.Validate(form);
            var repo = _usersRepo;
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

        public async Task<ResponseData<User?>> Delete(int? id)
        {
            if (id == null)
            {
                return new ResponseData<User?>("User id is required!");
            }
            return new ResponseData<User?>(await _usersRepo.Delete((int)id));
        }
    }
}
