using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Common;
using CareCare.Core.Application.Abstraction.Models.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.Vehicles
{
    public class VehicleController(IServiceManager serviceManager) : BaseApiController
    {
        [Authorize(Roles = Roles.User + "," + Roles.Technical)]
        [HttpPost("Create-Vehicle")]
        public async Task<ActionResult<VehicleToReturn>> CreateVehicle([FromBody] CreateVehicleDto createVehicleDto)
        {
            var result = await serviceManager.VehicleService.CreateVehicle(createVehicleDto);
            return Ok(result);
        }
        [Authorize(Roles = Roles.User + "," + Roles.Technical)]
        [HttpGet("Get-All-Vehicle-For-SpecificUser")]
        public async Task<ActionResult<Pagination<VehicleToReturn>>> GetAllVehicleForSpecificUser([FromQuery] SpecParams specParams)
        {
            var result = await serviceManager.VehicleService.GetAllVehicleForUser(User, specParams);
            return Ok(result);
        }
    }
}
