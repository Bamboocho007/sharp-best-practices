using Authentication_clone.Auth;
using Authentication_clone.Db;
using Authentication_clone.DTOs;
using Authentication_clone.Models;
using Authentication_clone.Validators;
using AutoMapper;

namespace Authentication_clone.ModelServices
{
    public class UserService
    {
        private readonly UsersRepo _usersRepo;
        private readonly IMapper _mapper;
        public UserService(UsersRepo usersRepo, IMapper mapper)
        {
            _usersRepo = usersRepo;
            _mapper = mapper;
        }

        public async Task<ResponseData<UserDto?>> GetInfo(string tokenString)
        {
            if (tokenString == null)
            {
                return new ResponseData<UserDto?>("Token not exists!");
            }

            var idFromClaims = JwtHelper.GetJWTTokenClaim(tokenString, "Id");
            int id = int.TryParse(idFromClaims, out id) ? id : 0;
            var user = await _usersRepo.GetById(id);

            if (user == null)
            {
                return new ResponseData<UserDto?>("User not exists!");
            }

            return new ResponseData<UserDto?>(_mapper.Map<UserDto>(user));
        }

        public async Task<ResponseData<UserDto?>> Create(UserForm form)
        {
            UserFormValidator validator = new ();
            var validated = validator.Validate(form);
            var repo = _usersRepo;

            if (await repo.GetByEmail(form.Email) != null)
            {
                return new ResponseData<UserDto?>("User with same email already exists!");
            }

            if (validated.IsValid)
            {
                User formUser = new() { Name = form.Name, Email = form.Email, Password = PasswordHelper.HashPassword(form.Password), Role = Role.Contributor};
                var user = _mapper.Map <UserDto>(await repo.Add(formUser));
                return new ResponseData<UserDto?>(user);
            }

            return new ResponseData<UserDto?>("Form data is not valid!", validated.Errors);
        }

        public async Task<ResponseData<UserDto?>> Update(UpdateUserForm form, int? userId)
        {
            if (userId == null)
            {
                return new ResponseData<UserDto?>("User id is required!");
            }

            UpdateUserFormValidator validator = new();
            var validated = validator.Validate(form);
            var repo = _usersRepo;
            var currentUser = await repo.GetById((int)userId);

            if (currentUser != null)
            {
                if(validated.IsValid)
                {
                    var updatedUser = await repo.Update(
                        new User { 
                            Id = currentUser.Id, 
                            Name = form.Name, 
                            Email = form.Email, 
                            Role = form.Role, 
                            Password = PasswordHelper.HashPassword(form.Password) 
                        }
                    );
                    return new ResponseData<UserDto?>(_mapper.Map<UserDto>(updatedUser));
                }

                return new ResponseData<UserDto?>("Form data is not valid!", validated.Errors);
            }

            return new ResponseData<UserDto?>("User no exists!");
        }

        public async Task<ResponseData<UserDto?>> Delete(int? id)
        {
            if (id == null)
            {
                return new ResponseData<UserDto?>("User id is required!");
            }
            return new ResponseData<UserDto?>(_mapper.Map<UserDto>(await _usersRepo.Delete((int)id)));
        }
    }
}
