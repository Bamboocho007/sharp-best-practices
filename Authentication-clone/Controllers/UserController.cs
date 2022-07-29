using Authentication_clone.Auth;
using Authentication_clone.DTOs;
using Authentication_clone.Helpers;
using Authentication_clone.ModelServices;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Authentication_clone.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly UserService _userService;

        public UserController(UserService userService, LoginService loginService)
        {
            _loginService = loginService;
            _userService = userService;
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
            var user = await _userService.GetInfo(tokenString);
            return user != null ? Ok(user) : NotFound(); 
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
