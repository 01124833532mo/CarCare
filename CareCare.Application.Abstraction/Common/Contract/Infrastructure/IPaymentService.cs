using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;

namespace CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure
{
    public interface IPaymentService
    {
        public Task<ReturnRequestDto> CreateOrUpdatePaymentIntent(int orderid);
        Task UpdateOrderPaymentStatus(string requestBody, string header);


    }
}
