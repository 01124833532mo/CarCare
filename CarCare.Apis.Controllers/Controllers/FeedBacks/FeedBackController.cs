using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.FeedBack;
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
            var result = await _serviceManager.FeedBackService.CreateFeedBackAsync(feedBackDto);
            return Ok(result);
        }

        [HttpGet("GetFeedBack/{id}")]
        public async Task<ReturnFeedBackDto> GetFeedBack([FromRoute] int id)
        {
            var result = await _serviceManager.FeedBackService.GetFeedBackAsync(id);
            return result;
        }

        [HttpPut("UpdateFeedBack/{id}")]
        public async Task<ReturnFeedBackDto> UpdateFeedBack([FromRoute] int id, [FromBody] UpdatedFeedBackDto feedBackDto)
        {
            var result = await _serviceManager.FeedBackService.UpdateFeedBackAsync(id, feedBackDto);
            return result;
        }

        [HttpDelete("DeleteFeedBack/{id}")]
        public async Task<string> DeleteFeedBack([FromRoute] int id)
        {
            var result = await _serviceManager.FeedBackService.DeleteFeedBackAsync(id);
            return result;
        }
    }
}
