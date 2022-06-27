using AutoMapper;
using email_app_api.Models;

namespace email_app_api.Services
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<UserEntity, User>()
                .ReverseMap();

            CreateMap<UserEntity, LoginResponse>();
        }
    }
}
