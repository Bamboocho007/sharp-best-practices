using Authentication_clone.DTOs;
using MediatR;

namespace Authentication_clone.Application.Commands
{
    public class DeleteUserCommand : IRequest<ResponseData<UserDto?>>
    {
        public string RouteId { get; set; }
    }
}
