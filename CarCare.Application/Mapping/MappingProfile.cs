using AutoMapper;
using CarCare.Core.Domain.Entities.Identity;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using Microsoft.AspNetCore.Identity;

namespace CarCare.Core.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<IdentityRole, RolesToReturn>().ReverseMap();
            CreateMap<IdentityRole, RoleDto>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>().ReverseMap();
            CreateMap<ApplicationUser, TechDto>().ReverseMap();

        }
    }
}
