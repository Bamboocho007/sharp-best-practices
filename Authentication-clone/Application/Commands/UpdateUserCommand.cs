using Authentication_clone.DTOs;
using MediatR;

namespace Authentication_clone.Application.Commands
{
    public class UpdateUserCommand: IRequest<ResponseData<UserDto?>>
    {
        public UpdateUserForm Form { get; set; }
        public string RouteId { get; set; }
    }
}
