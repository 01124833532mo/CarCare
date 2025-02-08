using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Common;
using CareCare.Core.Application.Abstraction.Models.FeedBack;
using CareCare.Core.Application.Abstraction.Models.Vehicles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.FeedBacks
{
	[Authorize(Roles = Roles.User + "," + Roles.Technical)]
	public class FeedBackController(IServiceManager _serviceManager) : BaseApiController
	{
		[HttpPost("CreateFeedBack")]
		public async Task<ActionResult<ReturnFeedBackDto>> CreateFeedBack([FromBody] CreateFeedBackDto feedBackDto)
		{
			var result = await _serviceManager.FeedBackService.CreateFeedBackAsync(User, feedBackDto);
			return Ok(result);
		}

		[HttpGet("GetAvarageRating")]
		public async Task<ActionResult<decimal>> GetAvgRating()
		{
			var result = await _serviceManager.FeedBackService.GetAvgRating();
			return Ok(result);
		}

		[HttpGet("GetAllFeedBacks")]
		public async Task<ActionResult<Pagination<VehicleToReturn>>> GetAllFeedBacks([FromQuery] SpecParams specParams)
		{
			var result = await _serviceManager.FeedBackService.GetAllFeedBackAsync(specParams);
			return Ok(result);
		}

		[HttpGet("GetFeedBack/{id}")]
		public async Task<ActionResult<ReturnFeedBackDto>> GetFeedBack([FromRoute] int id)
		{
			var result = await _serviceManager.FeedBackService.GetFeedBackAsync(id);
			return Ok(result);
		}

		[HttpPut("UpdateFeedBack/{id}")]
		public async Task<ActionResult<ReturnFeedBackDto>> UpdateFeedBack([FromRoute] int id, [FromBody] UpdatedFeedBackDto feedBackDto)
		{
			var result = await _serviceManager.FeedBackService.UpdateFeedBackAsync(User, id, feedBackDto);
			return Ok(result);
		}

		[HttpDelete("DeleteFeedBack/{id}")]
		public async Task<ActionResult<string>> DeleteFeedBack([FromRoute] int id)
		{
			var result = await _serviceManager.FeedBackService.DeleteFeedBackAsync(User, id);
			return Ok(result);
		}
	}
}
