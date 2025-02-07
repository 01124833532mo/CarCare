using AutoMapper;
using CarCare.Core.Domain.Entities.FeedBacks;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Core.Domain.Entities.Vehicles;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using CareCare.Core.Application.Abstraction.Models.FeedBack;
using CareCare.Core.Application.Abstraction.Models.ServiceTypes;
using CareCare.Core.Application.Abstraction.Models.Vehicles;
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
            CreateMap<ApplicationUser, TechDto>()
                .ForMember(dest => dest.ServiceName, option => option.MapFrom(src => src.ServiceType!.Name))
                .ReverseMap();

            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<Vehicle, VehicleToReturn>()
                .ForMember(dest => dest.FullName, option => option.MapFrom(src => src.User.FullName));

            CreateMap<CreateFeedBackDto, FeedBack>();
            CreateMap<FeedBack, ReturnFeedBackDto>();
            CreateMap<ServiceType, ServiceTypeToReturn>()
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom<ServiceTypePictureUrlResolver>());

        }
    }
}
