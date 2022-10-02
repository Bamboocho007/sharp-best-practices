using Authentication_clone.DTOs;
using Authentication_clone.ModelServices;
using MediatR;

namespace Authentication_clone.Application.Commands
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseData<UserDto>>
    {
        private readonly UserService _userService;
        public CreateUserCommandHandler(UserService userService) {
            _userService = userService;
        }

        public async Task<ResponseData<UserDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.Create(request.Form);
        }
    }
}
