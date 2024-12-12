using AutoMapper;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Models.Auth;
using CareCare.Core.Application.Abstraction.Services.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Application.Services
{
    public class AuthService(RoleManager<IdentityRole> roleManager,IMapper mapper) : IAuthService
    {
        public async Task CreateRoleAsync(RoleDto roleDto)
        {
            var roleExsits = await roleManager.RoleExistsAsync(roleDto.Name);

            if (!roleExsits)
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleDto.Name.Trim()));

            }
            else
            {
                throw new BadRequestExeption("This Role already Exists");
            }


        }



        public async Task<IEnumerable<RolesToReturn>> GetRolesAsync()
        {

            var roles = await roleManager.Roles.ToListAsync();
            var result = mapper.Map<IEnumerable<RolesToReturn>>(roles);
            return result;

        }


        public async Task UpdateRole(string id, RoleDto roleDto)
        {
            var roleExsists = await roleManager.RoleExistsAsync(roleDto.Name);
            if (!roleExsists)
            {
                var role =await roleManager.FindByIdAsync(id);
                role.Name = roleDto.Name;
                await roleManager.UpdateAsync(role);
            }
            else
            {
                throw new BadRequestExeption("this Role Already is Exsists");
            }
        }

        public async Task DeleteRole(string id)
        {
            var role= await roleManager.FindByIdAsync(id);
            if(role is null)
            {
                throw new NotFoundExeption(nameof(role), id);
            }
            await roleManager.DeleteAsync(role!);
        }

       
    }
}
