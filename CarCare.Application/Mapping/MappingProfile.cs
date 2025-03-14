using AutoMapper;
using CarCare.Core.Domain.Entities.Contacts;
using CarCare.Core.Domain.Entities.FeedBacks;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Core.Domain.Entities.Orders.ServicesDetails;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Core.Domain.Entities.Vehicles;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using CareCare.Core.Application.Abstraction.Models.Contacts;
using CareCare.Core.Application.Abstraction.Models.FeedBack;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;
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
                .ForMember(dest => dest.Profit, option => option.MapFrom(src => src.TechProfit))
                .ReverseMap();


            CreateMap<CreateVehicleDto, Vehicle>();
            CreateMap<Vehicle, VehicleToReturn>()
                .ForMember(dest => dest.FullName, option => option.MapFrom(src => src.User.FullName));

            CreateMap<CreateFeedBackDto, FeedBack>();
            CreateMap<FeedBack, ReturnFeedBackDto>()
                .ForMember(dest => dest.UserName, option => option.MapFrom(src => src.User.FullName));


            CreateMap<ServiceType, ServiceTypeToReturn>()
                .ForMember(dest => dest.PictureUrl, option => option.MapFrom<ServiceTypePictureUrlResolver>());

            CreateMap<CreateContactDto, Contact>();
            CreateMap<Contact, ReturnContactDto>();

            CreateMap<CreateRequestDto, ServiceRequest>()
      .ForMember(dest => dest.TireSize, option => option.MapFrom(src =>
          src.TireSize != null ? (TireSize)Enum.Parse(typeof(TireSize), src.TireSize.Replace(" ", ""), true) : (TireSize?)null))

      .ForMember(dest => dest.BettaryType, option => option.MapFrom(src =>
          src.BettaryType != null ? (BettaryType)Enum.Parse(typeof(BettaryType), src.BettaryType.Replace(" ", ""), true) : (BettaryType?)null))

      .ForMember(dest => dest.TypeOfFuel, option => option.MapFrom(src =>
          src.TypeOfFuel != null ? (TypeOfFuel)Enum.Parse(typeof(TypeOfFuel), src.TypeOfFuel.Replace(" ", ""), true) : (TypeOfFuel?)null))

      .ForMember(dest => dest.TypeOfOil, option => option.MapFrom(src =>
          src.TypeOfOil != null ? (TypeOfOil)Enum.Parse(typeof(TypeOfOil), src.TypeOfOil.Replace(" ", ""), true) : (TypeOfOil?)null))

      .ForMember(dest => dest.TypeOfWinch, option => option.MapFrom(src =>
          src.TypeOfWinch != null ? (TypeOfWinch)Enum.Parse(typeof(TypeOfWinch), src.TypeOfWinch.Replace(" ", ""), true) : (TypeOfWinch?)null))

      .ReverseMap();


            CreateMap<UpdateRequestDto, ServiceRequest>()
                .ForMember(dest => dest.TireSize, option => option.MapFrom(src => (TireSize)Enum.Parse(typeof(TireSize), src.TireSize ?? null!)))
                .ForMember(dest => dest.BettaryType, option => option.MapFrom(src => (BettaryType)Enum.Parse(typeof(BettaryType), src.BettaryType ?? null!)))
                .ForMember(dest => dest.TypeOfFuel, option => option.MapFrom(src => (TypeOfFuel)Enum.Parse(typeof(TypeOfFuel), src.TypeOfFuel ?? null!)))
                .ForMember(dest => dest.TypeOfOil, option => option.MapFrom(src => (TypeOfOil)Enum.Parse(typeof(TypeOfOil), src.TypeOfOil ?? null!)))
                .ForMember(dest => dest.TypeOfWinch, option => option.MapFrom(src => (TypeOfWinch)Enum.Parse(typeof(TypeOfWinch), src.TypeOfWinch ?? null!)))
                .ReverseMap()
                ;

            CreateMap<ServiceRequest, ReturnRequestDto>()
                .ForMember(dest => dest.TireSize, option => option.MapFrom(src => src.TireSize == 0 ? null : src.TireSize.ToString()))
                .ForMember(dest => dest.BettaryType, option => option.MapFrom(src => src.BettaryType == 0 ? null : src.BettaryType.ToString()))
                .ForMember(dest => dest.TypeOfFuel, option => option.MapFrom(src => src.TypeOfFuel == 0 ? null : src.TypeOfFuel.ToString()))
                .ForMember(dest => dest.TypeOfOil, option => option.MapFrom(src => src.TypeOfOil == 0 ? null : src.TypeOfOil.ToString()))
                .ForMember(dest => dest.TypeOfWinch, option => option.MapFrom(src => src.TypeOfWinch == 0 ? null : src.TypeOfWinch.ToString()))
                .ForMember(dest => dest.TechName, option => option.MapFrom(src => src.Technical.FullName))
                .ForMember(dest => dest.TechJop, option => option.MapFrom(src => src.Technical.ServiceType!.Name))
                .ForMember(dest => dest.UserName, option => option.MapFrom(src => src.User.FullName))
                .ForMember(dest => dest.BusnissStatus, option => option.MapFrom(src => src.BusnissStatus.ToString()))
                .ForMember(dest => dest.PaymentStatus, option => option.MapFrom(src => src.PaymentStatus.ToString()))
                .ReverseMap()
                ;

            //CreateMap<ApplicationUser, ReturnTechRequestDto>()
            //	.ForMember(dest => dest.ServiceName, option => option.MapFrom(src => src.ServiceType!.Name));

            CreateMap<(ApplicationUser Technician, double Distance), ReturnTechRequestDto>()
               .ForMember(dest => dest.ServiceName, option => option.MapFrom(src => src.Technician.ServiceType!.Name))
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Technician.Id))
               .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Technician.FullName))
               .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.Technician.PhoneNumber))
               .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Technician.Email))
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Technician.Type))
               .ForMember(dest => dest.NationalId, opt => opt.MapFrom(src => src.Technician.NationalId))
               .ForMember(dest => dest.Distance, opt => opt.MapFrom(src => src.Distance));

            CreateMap<UpdateTechnicalLocationDto, TechDto>();
        }

    }
}

