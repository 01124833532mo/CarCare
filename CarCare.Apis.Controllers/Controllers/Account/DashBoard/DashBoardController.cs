using CarCare.Apis.Controllers.Controllers.Base;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.Account.DashBoard
{
    public class DashBoardController(IServiceManager serviceManager) : BaseApiController
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

        [HttpGet("GetUsers")]
        public async Task<ActionResult> GetUsers()
        {
            var result = await serviceManager.AuthService.GetAllUsers();
            return Ok(result);
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDro createUserDro)
        {
            var result = await serviceManager.AuthService.CreateUser(createUserDro);
            return Ok(result);
        }

        [HttpGet("GetUser/{id}")]
        public async Task<ActionResult<UserRoleViewModel>> GetUser([FromRoute] string id)
        {
            var result = await serviceManager.AuthService.GetUser(id);
            return Ok(result);
        }

        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult<string>> DeleteUser([FromRoute] string id)
        {
            var result = await serviceManager.AuthService.DeleteUser(id);
            return Ok(result);
        }

        [HttpPut("UpdateUser/{id}")]
        public async Task<ActionResult<UserRoleViewModel>> UpdateUser([FromRoute] string id, [FromBody] UserEditeDto userEditeDto)
        {
            var result = await serviceManager.AuthService.EditeUser(id, userEditeDto);
            return Ok(result);
        }

    }
}
