using Authentication_clone.Application.Commands;
using Authentication_clone.Application.Queries;
using Authentication_clone.Auth;
using Authentication_clone.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication_clone.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly LoginService _loginService;
        private readonly IMediator _mediator;

        public UserController(LoginService loginService, IMediator mediator)
        {
            _loginService = loginService;
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserForm form)
        {
            var data = await _mediator.Send(new CreateUserCommand { Form = form});
            return data.Data != null ? Ok(data) : BadRequest(data);
        }

        [HttpGet]
        [AuthorizeJwt(Roles = $"{nameof(Role.Contributor)}, {nameof(Role.Administarator)}")]
        public async Task<ActionResult> GetUserInfo()
        {
            var tokenString = Request.Headers.Authorization
                              .ToString().Split(" ")[1];
            var user = await _mediator.Send(new GetUserQuery { TokenString = tokenString });

            if (user.Data != null)
            {
                return Ok(user);
            }

            return NotFound(user); 
        }

        [HttpPut("{id}")]
        [AuthorizeJwt(Roles = $"{nameof(Role.Contributor)}, {nameof(Role.Administarator)}")]
        public async Task<ActionResult> Update([FromBody] UpdateUserForm form)
        {
            var routeId = Request.RouteValues["id"];
            var data = await _mediator.Send(new UpdateUserCommand { Form = form, RouteId = (string?)routeId });
            return data?.Data != null ? Ok(data) : BadRequest(data);
        }

        [HttpDelete("{id}")]
        [AuthorizeJwt(Roles = $"{nameof(Role.Administarator)}")]
        public async Task<ActionResult> Delete()
        {
            var routeId = Request.RouteValues["id"];
            var data = await _mediator.Send(new DeleteUserCommand { RouteId = (string?)routeId });
            return data?.Data != null ? Ok(data) : BadRequest(data);
        }

        [HttpPost]
        public async Task<ActionResult> GetToken([FromBody] LoginForm loginForm)
        {
            var data = await _loginService.Login(loginForm);
            return data.Data != null ? Ok(data) : BadRequest(data);
        }
    }
}
