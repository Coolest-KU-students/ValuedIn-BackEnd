using AutoMapper;
using ValuedInBE.Users.Models;
using ValuedInBE.Users.Models.DTOs.Request;
using ValuedInBE.Users.Models.Entities;

namespace ValuedInBE.System.External.Tools.AutoMapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserCredentials, UserData>();
            CreateMap<NewUser, UserData>();
            CreateMap<UserCredentials, UserContext>();
        }
    }
}