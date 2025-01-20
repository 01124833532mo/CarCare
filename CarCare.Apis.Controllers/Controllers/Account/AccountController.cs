using CarCare.Apis.Controllers.Controllers.Base;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.Account
{

    public class AccountController(IServiceManager serviceManager) : BaseApiController
    {





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


        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var result = await serviceManager.AuthService.GetCurrentUser(User);
            return Ok(result);
        }


        [Authorize]
        [HttpGet("GetCurrentTechnical")]
        public async Task<ActionResult<TechDto>> GetCurrentTech()
        {
            var result = await serviceManager.AuthService.GetCurrentTechnical(User);
            return Ok(result);
        }

        [Authorize]
        [HttpGet("GetCurrentAdmin")]
        public async Task<ActionResult<TechDto>> GetCurrentAdmin()
        {
            var result = await serviceManager.AuthService.GetCurrentAdmin(User);
            return Ok(result);
        }

        [Authorize]

        [HttpPost("Change-Password")]
        public async Task<ActionResult<ChangePasswordDto>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            var result = await serviceManager.AuthService.ChangePasswordAsynce(User, changePasswordDto);
            return Ok(result);
        }
    }

}