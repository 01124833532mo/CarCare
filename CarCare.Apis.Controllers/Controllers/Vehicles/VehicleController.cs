using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.Vehicles
{
    public class VehicleController(IServiceManager serviceManager) : BaseApiController
    {
        [Authorize(Roles = Roles.User + "," + Roles.Technical)]
        [HttpPost("Create-Vehicle")]
        public async Task<ActionResult<CreateVehicleToReturn>> CreateVehicle([FromBody] CreateVehicleDto createVehicleDto)
        {
            var result = await serviceManager.VehicleService.CreateVehicle(createVehicleDto);
            return Ok(result);
        }
    }
}
