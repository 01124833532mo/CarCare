using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.FeedBack;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Apis.Controllers.Controllers.FeedBacks
{
	public class FeedBackController(IServiceManager _serviceManager) : BaseApiController
	{
		[Authorize(Roles = Roles.User + "," + Roles.Technical)]
		[HttpPost("CreateFeedBack")]
		public async Task<ActionResult<ReturnFeedBackDto>> CreateFeedBack([FromBody] CreateFeedBackDto feedBackDto)
		{
			var result = await _serviceManager.FeedBackService.CreateFeedBackAsync(feedBackDto);
			return Ok(result);
		}

		[HttpGet("GetFeedBack")]
		public async Task<ReturnFeedBackDto> GetFeedBack(int id)
		{
			var result = await _serviceManager.FeedBackService.GetFeedBackAsync(id);
			return result;
		}

		[HttpPut("UpdateFeedBack")]
		public async Task<ReturnFeedBackDto> UpdateFeedBack(UpdatedFeedBackDto feedBackDto)
		{
			var result = await _serviceManager.FeedBackService.UpdateFeedBackAsync(feedBackDto);
			return result;
		}

		[HttpDelete("DeleteFeedBack")]
		public async Task<string> DeleteFeedBack(int id)
		{
			var result = await _serviceManager.FeedBackService.DeleteFeedBackAsync(id);
			return result;
		}
	}
}
