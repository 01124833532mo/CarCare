using CarCare.Apis.Controllers.Controllers.Base;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.ServiceTypes;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.ServiceTypes
{
    public class ServiceTypesController(IServiceManager _serviceManager) : BaseApiController
    {
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<ServiceTypeToReturn>>> GetAllServiceTypes()
        {
            var result = await _serviceManager.ServiceTypeService.GetAllServicesTypes();
            return Ok(result);
        }
        [HttpGet("GetServiceType/{id}")]
        public async Task<ActionResult<ServiceTypeToReturn>> GetServiceType([FromRoute] int id)
        {
            var result = await _serviceManager.ServiceTypeService.GetServiceType(id);
            return Ok(result);
        }
    }
}
