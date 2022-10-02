using Authentication_clone.DTOs;
using Authentication_clone.Helpers;
using Authentication_clone.ModelServices;
using MediatR;

namespace Authentication_clone.Application.Commands
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, ResponseData<UserDto?>>
    {
        private readonly UserService _userService;

        public DeleteUserCommandHandler(UserService userService)
        {
            _userService = userService;
        }
        public async Task<ResponseData<UserDto?>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            return await _userService.Delete(NullableHelpers.TryParseNullableInt(request.RouteId));
        }
    }
}
