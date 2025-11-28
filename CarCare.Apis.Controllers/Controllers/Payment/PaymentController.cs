using CarCare.Apis.Controllers.Controllers.Base;
using CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Controllers.Controllers.Payment
{
    public class PaymentController(IPaymentService paymentService) : BaseApiController
    {
        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {


            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();


            await paymentService.UpdateOrderPaymentStatus(json, Request.Headers["Stripe-Signature"]!);
            return Ok();
        }

    }
}
