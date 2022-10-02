using Authentication_clone.DTOs;
using Authentication_clone.ModelServices;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Authentication_clone.Application.Queries
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ResponseData<UserDto?>>
    {
        private readonly UserService _userService;
        private readonly IDistributedCache _chache;

        public GetUserQueryHandler(UserService userService, IDistributedCache chache)
        {
            _userService = userService;
            _chache = chache;
        }

        public async Task<ResponseData<UserDto?>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            var chacheedInfoBytes = await _chache.GetAsync($"userInfo-{request.TokenString}", cancellationToken);
            if (chacheedInfoBytes != null)
            {
                var userString = System.Text.Encoding.UTF8.GetString(chacheedInfoBytes);
                var deserializedData = JsonSerializer.Deserialize<ResponseData<UserDto?>>(userString);
                if (deserializedData != null)
                {
                    return deserializedData;
                }
            }

            var data = await _userService.GetInfo(request.TokenString);
            var chacheOptions = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(30))
            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(1));
            var userBytes = System.Text.Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
            await _chache.SetAsync($"userInfo-{request.TokenString}", userBytes, chacheOptions);
            return data;
        }
    }
}
