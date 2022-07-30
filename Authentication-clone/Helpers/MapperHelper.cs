using Authentication_clone.DTOs;
using Authentication_clone.Models;
using AutoMapper;

namespace Authentication_clone.Helpers
{
    public static class MapperHelper
    {
        public static IMapper GetMapper(bool isDevelopment)
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<User, UserDto>();
            });

            if (isDevelopment)
            {
                configuration.AssertConfigurationIsValid();
            }

            return configuration.CreateMapper();
        } 
    }
}
