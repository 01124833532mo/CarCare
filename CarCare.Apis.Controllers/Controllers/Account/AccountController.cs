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