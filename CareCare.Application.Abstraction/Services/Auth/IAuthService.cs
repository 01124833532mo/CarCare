using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Technicals;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPasswordByEmailDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.TechLocation;
using CareCare.Core.Application.Abstraction.Models.Auth.UpdatingUsersDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using System.Security.Claims;

namespace CareCare.Core.Application.Abstraction.Services.Auth
{
    public interface IAuthService
    {
        #region Role

        Task<IEnumerable<RolesToReturn>> GetRolesAsync();

        Task<RolesToReturn> CreateRoleAsync(RoleDtoBase roleDto);

        Task DeleteRole(string id);

        Task<RolesToReturn> UpdateRole(string id, RoleDtoBase roleDto);

        #endregion


        #region Sign (in-up)

        Task<BaseUserDto> LoginAsync(LoginDto loginDto);

        Task<BaseUserDto> RegisterAsync(RegisterDto userRegisterDto);
        Task<TechDto> GetTechLocationAsync(ClaimsPrincipal claimsPrincipaldouble, TechLocationDto techLocationDto);


        #endregion


        #region Dashboard
        Task<IEnumerable<UserViewModel>> GetAllUsers();
        Task<UserDto> CreateUser(CreateUserDro createUserDro);

        Task<UserRoleViewModel> GetUser(string id);

        Task<string> DeleteUser(string id);

        Task<UserRoleViewModel> EditeUser(string id, EditDashDto viewModel);




        Task<IEnumerable<TechViewModel>> GetAllTechnicals();

        Task<TechDto> CreateTech(CreateTechnicalDto createTechnicalDto);
        Task<TechRoleViewModel> GetTechnical(string id);

        Task<string> DeleteTechnical(string id);
        Task<TechRoleViewModel> EditeTechnical(string id, EditDashDto viewModel);




        Task<ChangePasswordToReturn> ChangePasswordAsync(ClaimsPrincipal claims, ChangePasswordDto changePasswordDto);


        Task<BaseUserDto> GetRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default);

        Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default);

        #endregion


        #region Confirmation (Email - Phone)

        Task<SuccessDto> SendCodeByEmailasync(SendCodeByEmailDto emailDto);

        Task<SuccessDto> ForgetPasswordByPhoneAsync(ForgetPasswordByPhoneDto forgetPasswordDto);

        Task<SuccessDto> VerifyCodeByEmailAsync(ResetCodeConfirmationByEmailDto resetCodeDto);

        Task<SuccessDto> VerifyCodeByPhoneAsync(ResetCodeConfirmationByPhoneDto resetCode);

        Task<UserDto> ResetPasswordByEmailAsync(ResetPasswordByEmailDto resetCodeDto);

        Task<UserDto> ResetPasswordByPhoneAsync(ResetPasswordByPhoneDto resetPasswordDto);

        Task<SuccessDto> ConfirmEmailAsync(ConfirmationEmailCodeDto codeDto);

        Task<SuccessDto> ConfirmPhoneAsync(ConfirmationPhoneCodeDto codeDto);

        #endregion

        Task<BaseUserDto> UpdateAppUserBySelf(ClaimsPrincipal claims, UpdateTechDto appUserDto);
        Task<TechDto> UpdateOrSetTechnicalLocation(UpdateTechnicalLocationDto request);


        Task<string> RateTechnical(ClaimsPrincipal claims, decimal rate, string technicalid);


        Task<BaseUserDto> GetCurrentUserByRole(ClaimsPrincipal claims);

    }
}
