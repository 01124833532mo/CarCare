using CarCare.Apis.Controllers.Controllers.Base;
using CarCare.Shared.ErrorModoule.Errors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Apis.Controllers.Controllers.Common
{
    [ApiController]
    [Route("Errors/{Code}")]
    [ApiExplorerSettings(IgnoreApi = false)]
    public class ErrorsController :BaseApiController
    {
        [HttpGet]
        public IActionResult Error(int Code)
        {
            if (Code == (int)HttpStatusCode.NotFound)
            {
                var respnse = new ApiResponse((int)HttpStatusCode.NotFound, $"the requested endpoint  is not found");
                return NotFound(respnse);
            }
            return StatusCode(Code, new ApiResponse(Code));
        }
    }
}
