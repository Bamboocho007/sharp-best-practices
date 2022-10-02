using Authentication_clone.DTOs;
using MediatR;

namespace Authentication_clone.Application.Queries
{
    public class GetUserQuery: IRequest<ResponseData<UserDto?>>
    {
        public string TokenString { get; set; } = "";
    }
}
