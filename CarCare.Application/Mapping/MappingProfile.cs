using AutoMapper;
using CareCare.Core.Application.Abstraction.Models.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Application.Mapping
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap< IdentityRole, RolesToReturn>().ReverseMap();
            CreateMap<IdentityRole, RoleDto>().ReverseMap();

        }
    }
}
