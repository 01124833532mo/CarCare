using CareCare.Core.Application.Abstraction.Models.Auth;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {

         Task<IEnumerable<RolesToReturn>> GetRolesAsync();

        Task CreateRoleAsync(RoleDto roleDto);

        Task DeleteRole(string id);

        Task UpdateRole(string id,RoleDto roleDto);

    }
}
