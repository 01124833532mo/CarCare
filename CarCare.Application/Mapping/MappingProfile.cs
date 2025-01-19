using AutoMapper;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using Microsoft.AspNetCore.Identity;

namespace CarCare.Core.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityRole, RolesToReturn>().ReverseMap();
            CreateMap<IdentityRole, RoleDto>().ReverseMap();

        }
    }
}
