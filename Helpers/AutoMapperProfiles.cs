using Advansio.API.Dtos;
using Advansio.API.Models;
using AutoMapper;

namespace Advansio.API.Helpers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserForRegisterDto, User>();
            CreateMap<User, UserForListDto>();
        }
    }
}