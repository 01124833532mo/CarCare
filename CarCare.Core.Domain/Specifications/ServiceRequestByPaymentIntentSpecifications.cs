using CarCare.Core.Domain.Entities.Orders;

namespace CarCare.Core.Domain.Specifications
{
    public class ServiceRequestByPaymentIntentSpecifications : BaseSpecification<ServiceRequest, int>
    {
        public ServiceRequestByPaymentIntentSpecifications(string paymentintent) : base(
                    order => order.PaymentIntentId == paymentintent
            )
        {

        }
    }
}
