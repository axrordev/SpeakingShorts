using Amazon.S3.Model;
using AutoMapper;
using SpeakingShorts.Domain.Entities.Users;
using SpeakingShorts.Domain.Entities;
using SpeakingShorts.WebApi.Models.Users;

namespace SpeakingShorts.WebApi.MappingProfile
{

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User and related entities
        CreateMap<UserRegisterModel, User>();
        CreateMap<UserUpdateModel, User>();
        CreateMap<User, UserViewModel>();
        CreateMap<User, LoginViewModel>();
    }
}
}
