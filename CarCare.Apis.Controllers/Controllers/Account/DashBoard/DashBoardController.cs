using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Common;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Technicals;
using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using CareCare.Core.Application.Abstraction.Models.Contacts;
using CareCare.Core.Application.Abstraction.Models.ServiceTypes;
using CareCare.Core.Application.Abstraction.Models.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.Account.DashBoard
{
	[Authorize(Roles = Roles.Admin)]
	public class DashBoardController(IServiceManager serviceManager) : BaseApiController
	{

		[HttpGet("GetRoles")]
		public async Task<ActionResult> GetRoles()
		{
			var result = await serviceManager.AuthService.GetRolesAsync();
			return Ok(result);
		}

		[HttpPost("CreateRole")]
		public async Task<ActionResult> CreateRole(RoleDtoBase roleDto)
		{
			var result = await serviceManager.AuthService.CreateRoleAsync(roleDto);
			return Ok(result);

		}
		[HttpDelete("DeleteRole/{id}")]
		public async Task<ActionResult> DeleteRole(string id)
		{
			await serviceManager.AuthService.DeleteRole(id);
			return Ok("Delete Successfully");
		}
		[HttpPut("UpdateRole/{id}")]
		public async Task<ActionResult> UpdateRole(string id, RoleDtoBase roleDto)
		{
			var result = await serviceManager.AuthService.UpdateRole(id, roleDto);
			return Ok(result);
		}
		[AllowAnonymous]
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
		[AllowAnonymous]
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

		[AllowAnonymous]
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
		[AllowAnonymous]

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
		[AllowAnonymous]
		[HttpDelete("Delete-Vehicle/{id}")]
		public async Task<ActionResult<string>> DeleteVehicle([FromRoute] int id)
		{
			var result = await serviceManager.VehicleService.DeleteVehicle(id);
			return Ok(result);
		}
		[AllowAnonymous]

		[HttpGet("Get-Vehicle/{id}")]
		public async Task<ActionResult<string>> GetVehicle([FromRoute] int id)
		{
			var result = await serviceManager.VehicleService.GetVehicle(id);
			return Ok(result);
		}

		[HttpGet("Get-All-Vehicle")]
		public async Task<ActionResult<Pagination<VehicleToReturn>>> GetAllVehicle([FromQuery] SpecParams specParams)
		{
			var result = await serviceManager.VehicleService.GetAllVehicles(specParams);
			return Ok(result);
		}

		[HttpGet("GetAllFeedBacks")]
		public async Task<ActionResult<Pagination<VehicleToReturn>>> GetAllFeedBacks([FromQuery] SpecParams specParams)
		{
			var result = await serviceManager.FeedBackService.GetAllFeedBackAsync(specParams);
			return Ok(result);
		}

		[HttpGet("GetAvarageRating")]
		public async Task<ActionResult<decimal>> GetAvgRating()
		{
			var result = await serviceManager.FeedBackService.GetAvgRating();
			return Ok(result);
		}

		[HttpDelete("Delete-ServiceType/{id}")]
		public async Task<ActionResult<string>> DeleteServiceType([FromRoute] int id)
		{
			var result = await serviceManager.ServiceTypeService.DeleteServiceType(id);
			return Ok(result);
		}
		[HttpPost("Create-Service-Type")]
		public async Task<ActionResult<ServiceTypeToReturn>> CreateServiceType([FromForm] ServiceTypeDto createService)
		{
			var result = await serviceManager.ServiceTypeService.CreateServiceType(createService);
			return Ok(result);
		}



		[HttpPut("Update-Service-Type/{id}")]
		public async Task<ActionResult<ServiceTypeToReturn>> UpdateServiceType([FromRoute] int id, [FromForm] ServiceTypeDto createService)
		{
			var result = await serviceManager.ServiceTypeService.UpdateServiceType(id, createService);
			return Ok(result);
		}

		[HttpPost("CreateMessage")]
		public async Task<ActionResult<ReturnContactDto>> CreateMessage([FromBody] CreateContactDto contactDto)
		{
			var result = await serviceManager.ContactService.CreateContactAsync(contactDto);
			return Ok(result);
		}

		[HttpPut("UpdateMessage/{id}")]
		public async Task<ActionResult<ReturnContactDto>> UpdateContact([FromRoute] int id, CreateContactDto newContactDto)
		{
			var result = await serviceManager.ContactService.UpdateContactAsync(id, newContactDto);
			return Ok(result);
		}

		[HttpDelete("DeleteMessage/{id}")]
		public async Task<ActionResult<ReturnContactDto>> DeleteContact([FromRoute] int id)
		{
			var result = await serviceManager.ContactService.DeleteContactAsync(id);
			return Ok(result);
		}
	}

}