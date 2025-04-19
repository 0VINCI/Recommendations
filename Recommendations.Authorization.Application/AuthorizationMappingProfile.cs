using AutoMapper;
using Recommendations.Authorization.Core.Types;
using Recommendations.Authorization.Shared.DTO;

namespace Recommendations.Authorization.Application;

public class AuthorizationMappingProfile : Profile
{
    public AuthorizationMappingProfile()
    {
        CreateMap<User, UserDto>()
            .ForCtorParam("IdUser",     opt => opt.MapFrom(u => u.IdUser))
            .ForCtorParam("Name",   opt => opt.MapFrom(u => u.Name))
            .ForCtorParam("Surname",opt => opt.MapFrom(u => u.Surname))
            .ForCtorParam("Email",  opt => opt.MapFrom(u => u.Email));
    }
}
