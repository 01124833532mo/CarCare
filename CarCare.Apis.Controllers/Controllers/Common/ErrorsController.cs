using CarCare.Shared.ErrorModoule.Errors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LinkDev.Talabat.Apis.Controllers.Controllers.Common
{
    [ApiController]
    [Route("Errors/{Code}")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class ErrorsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Error(int Code)
        {
            if (Code == (int)HttpStatusCode.NotFound)
            {
                var respnse = new ApiResponse((int)HttpStatusCode.NotFound, $"the requested endpoint  is not found");
                return NotFound(respnse);
            }
            else if (Code == (int)HttpStatusCode.Forbidden)
            {
                var respnse = new ApiResponse((int)HttpStatusCode.Forbidden, $"the requested endpoint  is not Allowed");
                return StatusCode(Code, respnse);

            }
            else if (Code == (int)HttpStatusCode.MethodNotAllowed)
            {
                var respnse = new ApiResponse((int)HttpStatusCode.MethodNotAllowed, $"Method Not Allowed");
                return StatusCode(Code, respnse);

            }
            return StatusCode(Code, new ApiResponse(Code));
        }

    }
}
