using CareCare.Core.Application.Abstraction.Models.Auth;
using CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;

namespace CareCare.Core.Application.Abstraction.Services.Auth
{
	public interface IAuthService
    {

         Task<IEnumerable<RolesToReturn>> GetRolesAsync();

        Task CreateRoleAsync(RoleDto roleDto);

        Task DeleteRole(string id);

        Task UpdateRole(string id,RoleDto roleDto);

    	Task<BaseUserDto> LoginAsync(LoginDto loginDto);
		Task<UserDto> RegisterUserAsync(UserRegisterDto userRegisterDto);
		Task<TechDto> RegisterTechAsync(TechRegisterDto userRegisterDto);



	}
}
