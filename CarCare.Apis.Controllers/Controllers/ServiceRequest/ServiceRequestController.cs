using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Shared.Models.Roles;
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

        [Authorize(Roles = Roles.User)]
        [HttpPost("CreateRequestAutomatic")]
        public async Task<ActionResult<ReturnRequestDto>> CreateRequestAutomatic(CreateRequestDto requestDto)
        {
            var result = await serviceManager.RequestService.CreateRequestAutomatic(requestDto);
            return Ok(result);
        }
        [HttpPost("CreateRequestManually")]
        public async Task<ActionResult<ReturnRequestDto>> CreateRequestManually(CreateRequestDto requestDto)
        {
            var result = await serviceManager.RequestService.CreateRequestManually(requestDto);
            return Ok(result);
        }

        [HttpPut("Update-Technical-in-Request")]
        public async Task<ActionResult<ReturnRequestDto>> UpdateTechnicalInRequest([FromQuery] int RequestId, [FromQuery] int ServiceId, [FromQuery] string TechnicalId)
        {
            var result = await serviceManager.RequestService.UpdateTechnicalinRequest(RequestId, TechnicalId, ServiceId);
            return Ok(result);
        }
        [AllowAnonymous]

        [HttpDelete("DeleteRequestForUser/{RequestId}")]
        public async Task<ActionResult<string>> DeleteRequestForUser([FromRoute] int RequestId)
        {
            var result = await serviceManager.RequestService.DeleteRequest(RequestId);
            return Ok(result);
        }

        [HttpGet("GetRequest/{id}")]
        public async Task<ActionResult<ReturnRequestDto>> GetRequest([FromRoute] int id)
        {
            var result = await serviceManager.RequestService.ReturnRequest(id);
            return Ok(result);
        }
        [Authorize(Roles = Roles.User)]

        [HttpGet("GetAllRequests")]
        public async Task<ActionResult<ReturnRequestDto>> GetRequests()
        {
            var result = await serviceManager.RequestService.GetAllRequeststoUserForUser(User);
            return Ok(result);
        }
        [Authorize(Roles = Roles.User)]

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
        public async Task<ActionResult<string>> ReceivedRequest([FromQuery] int requestId, [FromQuery] BusnissStatus status)
        {
            var result = await serviceManager.RequestService.ReceivedRequest(requestId, status);
            return Ok(result);
        }
        [Authorize(Roles = Roles.Technical)]

        [HttpPut("AcceptRequest/{requestId}")]
        public async Task<ActionResult<string>> AcceptRequest([FromRoute] int requestId)
        {
            var result = await serviceManager.RequestService.AcceptRequest(requestId);
            return Ok(result);
        }
        [Authorize(Roles = Roles.Technical)]

        [HttpPut("RejectRequest/{requestId}")]
        public async Task<ActionResult<string>> RejectRequest([FromRoute] int requestId)
        {
            var result = await serviceManager.RequestService.RejectRequest(requestId);
            return Ok(result);
        }
        [Authorize(Roles = Roles.Technical)]

        [HttpPut("CompleteRequest/{requestId}")]
        public async Task<ActionResult<string>> CompleteRequest([FromRoute] int requestId)
        {
            var result = await serviceManager.RequestService.CompleteRequest(requestId);
            return Ok(result);
        }
        [Authorize(Roles = Roles.Technical)]

        [HttpGet("GetAllRequestsToTechnical")]
        public async Task<ActionResult<IEnumerable<ReturnRequestDto>>> GetAllRequestsToTechnical()
        {
            var result = await serviceManager.RequestService.GetAllRequestsToTechnical(User);
            return Ok(result);
        }
        [Authorize(Roles = Roles.Technical)]

        [HttpGet("GetAllPendingRequestsToTechnical")]
        public async Task<ActionResult<IEnumerable<ReturnRequestDto>>> GetAllPendingRequestsToTechnical([FromQuery] string? sort)
        {
            var result = await serviceManager.RequestService.GetAllPendingRequestsToTechnical(User, sort);
            return Ok(result);
        }

        [Authorize(Roles = Roles.Technical)]

        [HttpPut("TechincalBeActive")]
        public async Task<ActionResult<string>> TechincalBeActive()
        {
            var result = await serviceManager.RequestService.TechincalBeActive(User);
            return Ok(result);
        }
        [Authorize(Roles = Roles.Technical)]

        [HttpPut("TechincalBeInActive")]
        public async Task<ActionResult<string>> TechincalBeInActive()
        {
            var result = await serviceManager.RequestService.TechincalBeInActive(User);
            return Ok(result);
        }


        [HttpGet("CheckStatus/{requestId}")]
        public async Task<ActionResult<string>> CheckStatus([FromRoute] int requestId)
        {
            var result = await serviceManager.RequestService.CheckStatus(requestId);
            return Ok(result);
        }

        #endregion


    }
}
