using CarCare.Apis.Controllers.Controllers.Base;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Auth;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Apis.Controllers.Controllers.Account
{
    public class AccountController(IServiceManager serviceManager) : BaseApiController
    {



        [HttpGet("GetRoles")]
        public async Task<ActionResult> GetRoles()
        {
            var result =  await  serviceManager.AuthService.GetRolesAsync();
            return Ok(result);
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult> CreateRole(RoleDto roleDto)
        {
            await serviceManager.AuthService.CreateRoleAsync(roleDto);
            return Ok(roleDto);

        }
        [HttpDelete("DeleteRole/{id}")]
        public async Task<ActionResult> DeleteRole( string id)
        {
          await  serviceManager.AuthService.DeleteRole(id);
            return Ok();
        }
        [HttpPut("UpdateRole/{id}")]
        public async Task<ActionResult> UpdateRole(string id,RoleDto roleDto)
        {
            await serviceManager.AuthService.UpdateRole(id,roleDto);
            return Ok(roleDto);
        }
    }
}
