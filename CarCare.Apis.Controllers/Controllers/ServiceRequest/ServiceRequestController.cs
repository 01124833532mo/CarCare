using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Core.Domain.Entities.Orders;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Apis.Controllers.Controllers.ServiceRequest
{
	[Authorize]
	public class ServiceRequestController(IServiceManager serviceManager) : BaseApiController
	{
		#region User Request

		[HttpPost("CreateRequest")]
		public async Task<ActionResult<ReturnRequestDto>> CreateRequest(CreateRequestDto requestDto)
		{
			var result = await serviceManager.RequestService.CreateRequest(requestDto);
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
		[HttpGet("GetAvailableTechincals")]
		public async Task<ActionResult<ReturnTechRequestDto>> GetAvailableTechincals([FromHeader] int serviceId)
		{
			var result = await serviceManager.RequestService.GetActiveTechincals(serviceId);
			return Ok(result);
		}

		#endregion

		#region Techincal Received Request

		[HttpGet("ReceivedRequest")]
		public async Task<ActionResult<string>> ReceivedRequest([FromHeader] int requestId, [FromHeader] Status status)
		{
			var result = await serviceManager.RequestService.ReceivedRequest(requestId, status);
			return Ok(result);
		}

		#endregion


	}
}
