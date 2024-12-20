using CarCare.Apis.Controllers.Controllers.Base;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Auth;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.Account
{

	public class AccountController(IServiceManager serviceManager) : BaseApiController
    {



        [HttpGet("GetRoles")]
        public async Task<ActionResult> GetRoles()
        {
            var result = await serviceManager.AuthService.GetRolesAsync();
            return Ok(result);
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult> CreateRole(RoleDto roleDto)
        {
            await serviceManager.AuthService.CreateRoleAsync(roleDto);
            return Ok(roleDto);

        }
        [HttpDelete("DeleteRole/{id}")]
        public async Task<ActionResult> DeleteRole(string id)
        {
            await serviceManager.AuthService.DeleteRole(id);
            return Ok();
        }
        [HttpPut("UpdateRole/{id}")]
        public async Task<ActionResult> UpdateRole(string id, RoleDto roleDto)
        {
            await serviceManager.AuthService.UpdateRole(id, roleDto);
            return Ok(roleDto);
        }


        [HttpPost("login")]
        public async Task<ActionResult<BaseUserDto>> Login(LoginDto loginDto)
        {
            var result = await serviceManager.AuthService.LoginAsync(loginDto);
            return Ok(result);
        }

        [HttpPost("register/user")]
        public async Task<ActionResult<UserDto>> RegisterUser(UserRegisterDto registerDto)
        {
            var result = await serviceManager.AuthService.RegisterUserAsync(registerDto);
            return Ok(result);
        }

        [HttpPost("register/Technical")]
        public async Task<ActionResult<UserDto>> RegisterTechnical(TechRegisterDto registerDto)
        {
            var result = await serviceManager.AuthService.RegisterTechAsync(registerDto);
            return Ok(result);
        }

    }
    
}