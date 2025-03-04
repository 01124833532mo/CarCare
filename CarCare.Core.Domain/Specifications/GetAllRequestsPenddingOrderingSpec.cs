using CarCare.Core.Domain.Entities.Orders;

namespace CarCare.Core.Domain.Specifications
{
    public class GetAllRequestsPenddingOrderingSpec : BaseSpecification<ServiceRequest, int>
    {
        public GetAllRequestsPenddingOrderingSpec(string? sort)
        {
            AddIncludes();

            switch (sort)
            {
                case "Distance":
                    AddOrderBy(p => p.Distance);
                    break;

                case "Oldest":
                    AddOrderBy(p => p.CreatedOn);
                    break;

            }
        }



    }
}
