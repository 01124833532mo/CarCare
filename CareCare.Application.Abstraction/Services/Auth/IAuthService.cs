using CareCare.Core.Application.Abstraction.Models.Auth._Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Technicals;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users;
using CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using System.Security.Claims;

namespace CareCare.Core.Application.Abstraction.Services.Auth
{
	public interface IAuthService
	{

		Task<IEnumerable<RolesToReturn>> GetRolesAsync();

		Task CreateRoleAsync(RoleDto roleDto);

		Task DeleteRole(string id);

		Task UpdateRole(string id, RoleDto roleDto);

		Task<BaseUserDto> LoginAsync(LoginDto loginDto);
		Task<UserDto> RegisterUserAsync(UserRegisterDto userRegisterDto);
		Task<TechDto> RegisterTechAsync(TechRegisterDto userRegisterDto);

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


		Task<UserDto> GetCurrentUser(ClaimsPrincipal claims);
		Task<TechDto> GetCurrentTechnical(ClaimsPrincipal claims);

		Task<UserDto> GetCurrentAdmin(ClaimsPrincipal claims);

		Task<ChangePasswordToReturn> ChangePasswordAsynce(ClaimsPrincipal claims, ChangePasswordDto changePasswordDto);


		Task<BaseUserDto> GetRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default);

		Task<bool> RevokeRefreshTokenAsync(RefreshDto refreshDto, CancellationToken cancellationToken = default);


		Task<SuccessDto> ForgetPasswordAsync(ForgetPasswordDto forgetPasswordDto);


		Task<SuccessDto> VerifyCodeAsync(ResetCodeConfirmationDto resetCode);


		Task<UserDto> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);

		Task<SuccessDto> ConfirmationCodeSendAsync(ForgetPasswordDto confirmationCodeDto);



		Task<SuccessDto> ConfirmPhoneAsync(ConfirmationPhoneCodeDto codeDto);
	}
}
