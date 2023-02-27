using AutoMapper;
using ValuedInBE.Models;
using ValuedInBE.Models.DTOs.Requests.Users;
using ValuedInBE.Models.Users;

namespace ValuedInBE.AutoMapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCredentials, User>();
            CreateMap<NewUser, User>();
        }
    }
}
