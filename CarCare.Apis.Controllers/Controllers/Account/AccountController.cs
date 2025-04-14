using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPasswordByEmailDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.TechLocation;
using CareCare.Core.Application.Abstraction.Models.Auth.UpdatingUsersDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.Account
{

	public class AccountController(IServiceManager serviceManager) : BaseApiController
	{



		#region Sign (up - in)


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


		[HttpPut("Update-Technical-Location")]
		public async Task<ActionResult<TechDto>> UpdateTechLocation([FromBody] UpdateTechnicalLocationDto technicalLocationDto)
		{
			var result = await serviceManager.AuthService.UpdateOrSetTechnicalLocation(technicalLocationDto);
			return Ok(result);
		}

		#endregion


		#region Role


		[Authorize]
		[HttpGet("GetCurrentUserByRole")]
		public async Task<ActionResult<BaseUserDto>> GetCurrentUserByRole()
		{
			var result = await serviceManager.AuthService.GetCurrentUserByRole(User);
			return Ok(result);
		}

		#endregion


		#region Change_Password


		[Authorize]

		[HttpPost("Change-Password")]
		public async Task<ActionResult<ChangePasswordDto>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
		{
			var result = await serviceManager.AuthService.ChangePasswordAsync(User, changePasswordDto);
			return Ok(result);
		}

		#endregion


		#region Refresh Token

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
		#endregion


		#region Confirmation


		[HttpPost("SendCodeByEmail")]
		public async Task<ActionResult<SuccessDto>> SendCodeByEmail(SendCodeByEmailDto forgetPasswordDto)
		{
			var result = await serviceManager.AuthService.SendCodeByEmailasync(forgetPasswordDto);
			return Ok(result);
		}

		[HttpPost("ForgetPasswordPhone")]
		public async Task<ActionResult> ForgetPasswordPhone(ForgetPasswordByPhoneDto forgetPasswordDto)
		{
			var result = await serviceManager.AuthService.ForgetPasswordByPhoneAsync(forgetPasswordDto);
			return Ok(result);
		}

		[HttpPost("VerfiyCodeEmail")]
		public async Task<ActionResult<SuccessDto>> VerfiyCodeEmail(ResetCodeConfirmationByEmailDto resetCode)
		{
			var result = await serviceManager.AuthService.VerifyCodeByEmailAsync(resetCode);
			return Ok(result);
		}

		[HttpPost("VerfiyCodePhone")]
		public async Task<ActionResult> VerfiyCodePhone(ResetCodeConfirmationByPhoneDto resetCode)
		{
			var result = await serviceManager.AuthService.VerifyCodeByPhoneAsync(resetCode);
			return Ok(result);
		}

		[HttpPut("ResetPasswordEmail")]
		public async Task<ActionResult<UserDto>> ResetPasswordEmail(ResetPasswordByEmailDto resetPassword)
		{
			var result = await serviceManager.AuthService.ResetPasswordByEmailAsync(resetPassword);
			return Ok(result);
		}

		[HttpPut("ResetPasswordPhone")]
		public async Task<ActionResult> ResetPasswordPhone(ResetPasswordByPhoneDto resetPassword)
		{
			var result = await serviceManager.AuthService.ResetPasswordByPhoneAsync(resetPassword);
			return Ok(result);
		}


		[HttpPost("ConfirmEmail")]
		public async Task<ActionResult<SuccessDto>> ConfirmEmail(ConfirmationEmailCodeDto codeDto)
		{
			var result = await serviceManager.AuthService.ConfirmEmailAsync(codeDto);
			return Ok(result);
		}

		[HttpPost("ConfirmPhone")]
		public async Task<ActionResult> ConfirmPhone(ConfirmationPhoneCodeDto codeDto)
		{
			var result = await serviceManager.AuthService.ConfirmPhoneAsync(codeDto);
			return Ok(result);
		}
		#endregion


		#region Update (User - Technical)


		[Authorize(Roles = $"{Roles.Technical}, {Roles.User}")]

		[HttpPut("UpdateAppUser")]
		public async Task<ActionResult> UpdateAppUser([FromBody] UpdateTechDto updateAppUser)
		{
			var result = await serviceManager.AuthService.UpdateAppUserBySelf(User, updateAppUser);
			return Ok(result);
		}


		#endregion

		[Authorize(Roles = Roles.User)]
		[HttpPost("RateTechnical")]
		public async Task<ActionResult<string>> RateTechnical([FromQuery] decimal rate, [FromQuery] string technicalid)
		{
			var result = await serviceManager.AuthService.RateTechnical(User, rate, technicalid);
			return Ok(result);
		}
	}

}