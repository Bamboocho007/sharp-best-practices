using Authentication_clone.Auth;
using Authentication_clone.Db;
using Authentication_clone.DTOs;
using Authentication_clone.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Authentication_clone.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        IConfiguration _config;
        private readonly JwtSettings _jwtSettings;
        private readonly LoginService _loginService;

        public UserController(IConfiguration config, JwtSettings jwtSettings)
        {
            _config = config;
            _jwtSettings = jwtSettings;
            _loginService = new LoginService(jwtSettings, config);
        }

        [Route("{id}")]
        [HttpGet]
        public async Task<User> GetOne()
        {
            var param = (string?)Request.RouteValues["id"];
            int id = int.TryParse(param, out id) ? id : 0;
            return await new UsersService(_config).GetById(id);
        }

        [HttpGet]
        [AuthorizeJwt(Roles = $"{nameof(Role.Contributor)}")]
        public async Task<User> GetUserInfo()
        {
            var tokenString = Request.Headers.Authorization
                              .ToString().Split(" ")[1];
            var idFromClaims = JwtHelper.GetJWTTokenClaim(tokenString, "Id");
            int id = int.TryParse(idFromClaims, out id) ? id : 0;
            return await new UsersService(_config).GetById(id);
        }

        [HttpPost]
        public async Task<ActionResult> GetToken([FromBody] LoginForm loginForm)
        {
            var data = await _loginService.Login(loginForm);
            if (data.Data != null)
            {
                return Ok(JsonSerializer.Serialize(data));
            }
            return BadRequest(JsonSerializer.Serialize(data));
        }
    }
}
