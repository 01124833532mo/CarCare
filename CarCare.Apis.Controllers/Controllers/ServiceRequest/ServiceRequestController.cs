using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Core.Domain.Entities.Orders;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.ServiceRequest
{
    [Authorize]
    public class ServiceRequestController(IServiceManager serviceManager) : BaseApiController
    {
        #region User Request

        [HttpPost("CreateRequest")]
        public async Task<ActionResult<ReturnRequestDto>> CreateRequest(CreateRequestDto requestDto)
        {
            var result = await serviceManager.RequestService.CreateRequestAutomatic(requestDto);
            return Ok(result);
        }

        [HttpGet("GetRequest/{id}")]
        public async Task<ActionResult<ReturnRequestDto>> GetRequest([FromRoute] int id)
        {
            var result = await serviceManager.RequestService.ReturnRequest(id);
            return Ok(result);
        }

        [HttpGet("GetAllRequests")]
        public async Task<ActionResult<ReturnRequestDto>> GetRequests()
        {
            var result = await serviceManager.RequestService.GetAllRequeststoUserForUser(User);
            return Ok(result);
        }

        [HttpGet("GetNearestTechincals")]
        public async Task<ActionResult<IEnumerable<ReturnTechRequestDto>>> GetNearestTechincals([FromQuery] int serviceId, [FromQuery] double UserLatitude, [FromQuery] double UserLongitude)
        {
            var result = await serviceManager.RequestService.GetNearestTechnicals(serviceId, UserLatitude, UserLongitude);
            return Ok(result);
        }

        [HttpGet("GetAvailableTechincals")]
        public async Task<ActionResult<IEnumerable<ReturnTechRequestDto>>> GetAvailableTechincals([FromQuery] int serviceid, [FromQuery] double userlongitude, [FromQuery] double userlatidtude)
        {
            var result = await serviceManager.RequestService.GetActiveTechincals(serviceid, userlongitude, userlatidtude);
            return Ok(result);
        }

        #endregion

        #region Techincal Received Request

        [HttpGet("ReceivedRequest")]
        public async Task<ActionResult<string>> ReceivedRequest([FromQuery] int requestId, [FromQuery] Status status)
        {
            var result = await serviceManager.RequestService.ReceivedRequestAutomatic(User, requestId, status);
            return Ok(result);
        }

        [HttpPut("AcceptRequest")]
        public async Task<ActionResult<string>> AcceptRequest([FromQuery] int requestId)
        {
            var result = await serviceManager.RequestService.AcceptRequest(User, requestId);
            return Ok(result);
        }

        [HttpPut("RejectRequest")]
        public async Task<ActionResult<string>> RejectRequest([FromQuery] int requestId)
        {
            var result = await serviceManager.RequestService.RejectRequest(User, requestId);
            return Ok(result);
        }

        [HttpGet("GetAllRequestsToTechnical")]
        public async Task<ActionResult<IEnumerable<ReturnRequestDto>>> GetAllRequestsToTechnical()
        {
            var result = await serviceManager.RequestService.GetAllRequestsToTechnical(User);
            return Ok(result);
        }

        [HttpPut("TechincalBeActive")]
        public async Task<ActionResult<string>> TechincalBeActive()
        {
            var result = await serviceManager.RequestService.TechincalBeActive(User);
            return Ok(result);
        }

        [HttpPut("TechincalBeInActive")]
        public async Task<ActionResult<string>> TechincalBeInActive()
        {
            var result = await serviceManager.RequestService.TechincalBeInActive(User);
            return Ok(result);
        }

        #endregion


    }
}
