using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Core.Domain.Specifications;
using CarCare.Shared.ErrorModoule.Exeptions;
using CarCare.Shared.Models;
using CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Stripe;

namespace CarCare.Infrastructure.Payment_Service
{
    public class PaymentService(IOptions<StripSettings> stripsettings,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<PaymentService> logger)
        : IPaymentService
    {
        private readonly StripSettings _stripSettings = stripsettings.Value;

        public async Task<ReturnRequestDto> CreateOrUpdatePaymentIntent(int orderid)
        {
            StripeConfiguration.ApiKey = _stripSettings.SecretKey;
            var repo = unitOfWork.GetRepository<ServiceRequest, int>();

            var order = await repo.GetAsync(orderid);
            if (order is null) throw new NotFoundExeption("No Order Exsist For ", nameof(orderid));

            PaymentIntent? paymentIntent = null;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(order.PaymentIntentId))
            {// create New PaymentIntent

                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)order.ServicePrice * 100,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(options);     // integration with Stripe
                order.PaymentIntentId = paymentIntent.Id;
                order.ClientSecret = paymentIntent.ClientSecret;

            }
            else  // Update PaymentIntent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)order.ServicePrice * 100
                };

                await paymentIntentService.UpdateAsync(order.PaymentIntentId, options);      // integration with Stripe

            }
            repo.Update(order);
            var chickComplete = await unitOfWork.CompleteAsync() > 0;
            if (!chickComplete) throw new BadRequestExeption("There is an Error in Request");

            var result = mapper.Map<ReturnRequestDto>(order);

            return result;
        }

        public async Task UpdateOrderPaymentStatus(string requestBody, string header)
        {
            var stripeEvent = EventUtility.ConstructEvent(requestBody, header, _stripSettings.WebhookSecret);

            // Handle the event

            var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;
            ServiceRequest? order;
            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    order = await UpdatePaymentIntent(paymentIntent.Id, isPaid: true);
                    logger.LogInformation("Order is Succeeded With Payment IntentId:{0}", paymentIntent.Id);
                    break;
                case "payment_intent.payment_failed":
                    order = await UpdatePaymentIntent(paymentIntent.Id, isPaid: false);
                    logger.LogInformation("Order is !Succeeded With Payment IntentId:{0}", paymentIntent.Id);


                    break;
            }
        }

        private async Task<ServiceRequest> UpdatePaymentIntent(string paymentIntentId, bool isPaid)
        {
            var orderRepo = unitOfWork.GetRepository<ServiceRequest, int>();

            var spec = new ServiceRequestByPaymentIntentSpecifications(paymentIntentId);
            var order = await orderRepo.GetWithSpecAsync(spec);

            if (order is null) throw new NotFoundExeption(nameof(order), $"PaymentIntentId :{paymentIntentId}");

            if (isPaid)
                order.PaymentStatus = PaymentStatus.PaymentReceived;
            else
                order.PaymentStatus = PaymentStatus.PaymentFailed;


            orderRepo.Update(order);

            await unitOfWork.CompleteAsync();
            return order;

        }
    }
}
