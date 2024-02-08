using AutoMapper;
using CozyCub.Models.User.DTOs;
using CozyCub.Models.UserModels;

namespace CozyCub.Mappings
{
    public class AppMapper : Profile
    {

        public AppMapper()
        {
            CreateMap<User, UserRegisterDTO>().ReverseMap();
        }

    }
}
