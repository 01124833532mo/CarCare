using CarCare.Core.Domain.Entities.Vehicles;

namespace CarCare.Core.Domain.Specifications.SpecsHandlers.Vehicles
{
    public class VehicleWithUserSpecifications : BaseSpecification<Vehicle, int>
    {


        public VehicleWithUserSpecifications(string? sort, string? userid, int pageSize, int pageIndex, string? search)

        : base(
               p =>
                  (string.IsNullOrEmpty(search) || p.NormatizedVIN_Number.Contains(search))
                  &&
                  (string.IsNullOrEmpty(userid) || p.UserId == userid)


             )

        {

            AddIncludes();

            switch (sort)
            {
                case "modelDesc":
                    AddOrderByDesc(p => p.Model);
                    break;
                case "CreateOnAsec":
                    AddOrderBy(p => p.CreatedOn);
                    break;


                default:
                    AddOrderByDesc(p => p.CreatedOn);
                    break;
            }

            ApplyPagination((pageIndex - 1) * pageSize, pageSize);



        }
        public VehicleWithUserSpecifications(int id)
            : base(id)
        {
            AddIncludes();
        }

        private protected override void AddIncludes()
        {
            base.AddIncludes();

            Includes.Add(v => v.User);

        }
    }
}
