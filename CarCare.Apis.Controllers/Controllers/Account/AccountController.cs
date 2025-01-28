using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPasswordByEmailDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
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


        #endregion


        #region Role

        [Authorize(Roles = Roles.User + "," + Roles.Technical)]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var result = await serviceManager.AuthService.GetCurrentUser(User);
            return Ok(result);
        }


        [Authorize(Roles = Roles.User + "," + Roles.Technical)]
        [HttpGet("GetCurrentTechnical")]
        public async Task<ActionResult<TechDto>> GetCurrentTech()
        {
            var result = await serviceManager.AuthService.GetCurrentTechnical(User);
            return Ok(result);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpGet("GetCurrentAdmin")]
        public async Task<ActionResult<TechDto>> GetCurrentAdmin()
        {
            var result = await serviceManager.AuthService.GetCurrentAdmin(User);
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


        [HttpPost("ForgetPasswordEmail")]
        public async Task<ActionResult> ForgetPasswordEmail(ForgetPasswordByEmailDto forgetPasswordDto)
        {
            var result = await serviceManager.AuthService.ForgetPasswordByEmailasync(forgetPasswordDto);
            return Ok(result);
        }

        [HttpPost("ForgetPasswordPhone")]
        public async Task<ActionResult> ForgetPasswordPhone(ForgetPasswordByPhoneDto forgetPasswordDto)
        {
            var result = await serviceManager.AuthService.ForgetPasswordByPhoneAsync(forgetPasswordDto);
            return Ok(result);
        }

        [HttpPost("VerfiyCodeEmail")]
        public async Task<ActionResult> VerfiyCodeEmail(ResetCodeConfirmationByEmailDto resetCode)
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
        public async Task<ActionResult> ResetPasswordEmail(ResetPasswordByEmailDto resetPassword)
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

        [HttpPost("ConfirmationCodeEmail")]
        public async Task<ActionResult> ConfirmationCodeEmail(ForgetPasswordByEmailDto confirmationCode)
        {
            var result = await serviceManager.AuthService.ConfirmationCodeSendByEmailAsync(confirmationCode);
            return Ok(result);
        }


        [HttpPost("ConfirmationCodePhone")]
        public async Task<ActionResult> ConfirmationCodePhone(ForgetPasswordByPhoneDto confirmationCode)
        {
            var result = await serviceManager.AuthService.ConfirmationCodeSendByPhoneAsync(confirmationCode);
            return Ok(result);
        }

        [HttpPost("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(ConfirmationEmailCodeDto codeDto)
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

        [Authorize(Roles = Roles.User)]
        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUserByUser([FromBody] UpdateUserDto updateUser)
        {
            var result = await serviceManager.AuthService.UpdateUserByUser(User, updateUser);
            return Ok(result);
        }
        [Authorize(Roles = Roles.Technical)]

        [HttpPut("UpdateTech")]
        public async Task<ActionResult> UpdateTechByTech([FromBody] UpdateTechDto updateTech)
        {
            var result = await serviceManager.AuthService.UpdateTechByTech(User, updateTech);
            return Ok(result);
        }
        #endregion
    }

}