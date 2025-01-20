using CarCare.Apis.Controllers.Controllers.Base;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Technicals;
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
        public async Task<ActionResult<UserRoleViewModel>> UpdateUser([FromRoute] string id, [FromBody] EditDashDto userEditeDto)
        {
            var result = await serviceManager.AuthService.EditeUser(id, userEditeDto);
            return Ok(result);
        }


        [HttpGet("GetTechnicals")]
        public async Task<ActionResult> GetTechnicals()
        {
            var result = await serviceManager.AuthService.GetAllTechnicals();
            return Ok(result);
        }

        [HttpPost("CreateTechnical")]
        public async Task<ActionResult<TechDto>> CreateTech(CreateTechnicalDto createTechnicalDto)
        {
            var result = await serviceManager.AuthService.CreateTech(createTechnicalDto);
            return Ok(result);
        }

        [HttpGet("GetTechnical/{id}")]
        public async Task<ActionResult<TechRoleViewModel>> GetTech([FromRoute] string id)
        {
            var result = await serviceManager.AuthService.GetTechnical(id);
            return Ok(result);
        }

        [HttpDelete("DeleteTechnical/{id}")]
        public async Task<ActionResult<string>> DeleteTech([FromRoute] string id)
        {
            var result = await serviceManager.AuthService.DeleteTechnical(id);
            return Ok(result);
        }


        [HttpPut("UpdateTechnical/{id}")]
        public async Task<ActionResult<TechRoleViewModel>> UpdateTech([FromRoute] string id, [FromBody] EditDashDto EditTechDto)
        {
            var result = await serviceManager.AuthService.EditeTechnical(id, EditTechDto);
            return Ok(result);
        }

    }
}
