using Authentication_clone.DTOs;
using MediatR;

namespace Authentication_clone.Application.Commands
{
    public class CreateUserCommand: IRequest<ResponseData<UserDto>>
    {
        public UserForm Form { get; set; }
    }
}
