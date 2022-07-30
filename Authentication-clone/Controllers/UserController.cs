using Authentication_clone.Auth;
using Authentication_clone.DTOs;
using Authentication_clone.Helpers;
using Authentication_clone.ModelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Authentication_clone.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly UserService _userService;
        private readonly IDistributedCache _chache;

        public UserController(UserService userService, LoginService loginService, IDistributedCache chache)
        {
            _loginService = loginService;
            _userService = userService;
            _chache = chache;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserForm form)
        {
            var data = await _userService.Create(form);
            var serialized = JsonSerializer.Serialize(data);
            return data.Data != null ? Ok(serialized) : BadRequest(serialized);
        }

        [HttpGet]
        [AuthorizeJwt(Roles = $"{nameof(Role.Contributor)}, {nameof(Role.Administarator)}")]
        public async Task<ActionResult> GetUserInfo()
        {
            var tokenString = Request.Headers.Authorization
                              .ToString().Split(" ")[1];
            var chacheedInfoBytes = await _chache.GetAsync($"userInfo-{tokenString}");

            if (chacheedInfoBytes != null)
            {
                var userString = System.Text.Encoding.UTF8.GetString(chacheedInfoBytes);
                return Ok(userString);
            }

            var chacheOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(30))
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(1));

            var user = await _userService.GetInfo(tokenString);
            if (user != null)
            {
                var userBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(user));
                await _chache.SetAsync($"userInfo-{tokenString}", userBytes, chacheOptions);
                return Ok(user);
            }

            return NotFound(); 
        }

        [HttpPut("{id}")]
        [AuthorizeJwt(Roles = $"{nameof(Role.Contributor)}, {nameof(Role.Administarator)}")]
        public async Task<ActionResult> Update([FromBody] UpdateUserForm form)
        {
            var routeId = Request.RouteValues["id"];
            var data = await _userService.Update(form, NullableHelpers.TryParseNullableInt((string?)routeId));
            var serialized = JsonSerializer.Serialize(data);
            return data?.Data != null ? Ok(serialized) : BadRequest(serialized);
        }

        [HttpDelete("{id}")]
        [AuthorizeJwt(Roles = $"{nameof(Role.Administarator)}")]
        public async Task<ActionResult> Delete()
        {
            var routeId = Request.RouteValues["id"];
            var data = await _userService.Delete(NullableHelpers.TryParseNullableInt((string?)routeId));
            var serialized = JsonSerializer.Serialize(data);
            return data?.Data != null ? Ok(serialized) : BadRequest(serialized);
        }

        [HttpPost]
        public async Task<ActionResult> GetToken([FromBody] LoginForm loginForm)
        {
            var data = await _loginService.Login(loginForm);
            var serialized = JsonSerializer.Serialize(data);
            return data.Data != null ? Ok(serialized) : BadRequest(serialized);
        }
    }
}
