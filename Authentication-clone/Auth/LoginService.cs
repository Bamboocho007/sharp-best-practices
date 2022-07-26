using Authentication_clone.Db;
using Authentication_clone.DTOs;
using Authentication_clone.Validators;

namespace Authentication_clone.Auth
{
    public class LoginService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UsersRepo _usersRepo;
        private readonly IConfiguration _config;

        public LoginService(JwtSettings jwtSettings, IConfiguration config) {
            _jwtSettings = jwtSettings;
            _usersRepo = new UsersRepo(config);
            _config = config;
        }

        public async Task<ResponseData<TokenResponse>> Login(LoginForm form)
        {
            LoginValidator loginValidator = new();
            var validated = await loginValidator.ValidateAsync(form);

            if (validated.IsValid)
            {
                var user = await _usersRepo.GetByEmail(form.Email);
                if (user != null)
                {
                    if (PasswordHelper.VerifyPassword(user.Password, form.Password))
                    {
                        var token = JwtHelper.GetToken(user.Id, user.Role, _jwtSettings);
                        var responceData = new ResponseData<TokenResponse>(new TokenResponse { Token = token });
                        return responceData;
                    }
                    return new ResponseData<TokenResponse>("Password or email is wrong!");
                }
                return new ResponseData<TokenResponse>("There is no user with current email!");
            }

            var errors = new List<ErrorsDescription>();

            foreach (var error in validated.Errors)
            {
                errors.Add(new ErrorsDescription { Message = error.ErrorMessage, PropertyName = error.PropertyName });
            }
            return new ResponseData<TokenResponse>("Form data is not valid!", errors);
        }
    }
}
