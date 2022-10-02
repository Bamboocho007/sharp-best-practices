using Authentication_clone.DTOs;
using Authentication_clone.Helpers;
using Authentication_clone.ModelServices;
using MediatR;

namespace Authentication_clone.Application.Commands
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ResponseData<UserDto?>>
    {
        private readonly UserService _userService;

        public UpdateUserCommandHandler(UserService userService)
        {
            _userService = userService;
        }

        public async Task<ResponseData<UserDto?>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.Update(request.Form, NullableHelpers.TryParseNullableInt(request.RouteId));
        }
    }
}
