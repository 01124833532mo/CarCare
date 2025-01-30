using CarCare.Core.Domain.Entities.Vehicles;

namespace CarCare.Core.Domain.Specifications.SpecsHandlers.Vehicles
{
    public class VehicleWithFilterationForCountSpecifications : BaseSpecification<Vehicle, int>
    {

        public VehicleWithFilterationForCountSpecifications(string? userid, string? search) :
            base(
                   p =>
                  (string.IsNullOrEmpty(search) || p.NormatizedVIN_Number.Contains(search))
                  &&
                 (string.IsNullOrEmpty(userid) || p.UserId == userid)
                )
        {

        }


    }
}
