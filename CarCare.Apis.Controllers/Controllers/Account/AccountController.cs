using CarCare.Apis.Controllers.Controllers.Base;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.UpdatingUsersDtos;
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

		[HttpPost("Get-Refresh-Token")]

		public async Task<ActionResult<UserDto>> GetRefreshToken([FromBody] RefreshDto model)
		{
			var result = await serviceManager.AuthService.GetRefreshTokenAsync(model);
			return Ok(result);
		}
		[HttpPost("Revoke-Refresh-Token")]
		public async Task<ActionResult> RevokeRefreshToken([FromBody] RefreshDto model)
		{
			var result = await serviceManager.AuthService.RevokeRefreshTokenAsync(model);
			return result is false ? BadRequest("Operation Not Successed") : Ok("Revoed Successfully!");

		}

		[HttpPost("ForgetPassword")]
		public async Task<ActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
		{
			var result = await serviceManager.AuthService.ForgetPasswordAsync(forgetPasswordDto);
			return Ok(result);
		}

		[HttpPost("VerfiyCode")]
		public async Task<ActionResult> VerfiyCode(ResetCodeConfirmationDto resetCode)
		{
			var result = await serviceManager.AuthService.VerifyCodeAsync(resetCode);
			return Ok(result);
		}

		[HttpPut("ResetPassword")]
		public async Task<ActionResult> ResetPassword(ResetPasswordDto resetPassword)
		{
			var result = await serviceManager.AuthService.ResetPasswordAsync(resetPassword);
			return Ok(result);
		}

		[HttpPost("ConfirmationCode")]
		public async Task<ActionResult> ConfirmationCode(ForgetPasswordDto confirmationCode)
		{
			var result = await serviceManager.AuthService.ConfirmationCodeSendAsync(confirmationCode);
			return Ok(result);
		}

		[HttpPost("ConfirmPhone")]
		public async Task<ActionResult> ConfirmPhone(ConfirmationPhoneCodeDto codeDto)
		{
			var result = await serviceManager.AuthService.ConfirmPhoneAsync(codeDto);
			return Ok(result);
		}

		[HttpPut("UpdateUser")]
		public async Task<ActionResult> UpdateUserByUser(UpdateUserDto updateUser)
		{
			var result = await serviceManager.AuthService.UpdateUserByUser(User, updateUser);
			return Ok(result);
		}

		[HttpPut("UpdateTech")]
		public async Task<ActionResult> UpdateTechByTech(UpdateTechDto updateTech)
		{
			var result = await serviceManager.AuthService.UpdateTechByTech(User, updateTech);
			return Ok(result);
		}
	}

}