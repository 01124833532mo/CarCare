using CarCare.Core.Domain.Entities.Orders;

namespace CarCare.Core.Domain.Specifications
{
    public class ServiceRequestForSpecificTechnical : BaseSpecification<ServiceRequest, int>
    {
        public ServiceRequestForSpecificTechnical(string? technicalid) : base(

            order => order.TechId == technicalid


            )
        {

        }

    }
}
