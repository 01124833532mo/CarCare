using CarCare.Core.Domain.Contracts.Persistence.Vehicles;
using CarCare.Core.Domain.Entities.Vehicles;
using CarCare.Infrastructure.Persistence._Data;
using CarCare.Infrastructure.Persistence.Generic_Repository;

namespace CarCare.Infrastructure.Persistence.Repositories.Vehicles
{
    public class VehicleRepository : GenericRepository<Vehicle, int>, IVehicleRepository
    {
        private readonly CarCarIdentityDbContext _dbContext;

        public VehicleRepository(CarCarIdentityDbContext dbContext) : base(dbContext)
        {

            _dbContext = dbContext;
        }

        public bool CheckPlateNumberExist(string number)
        {
            var result = _dbContext.Vehicles.Where(e => e.PlateNumber.Contains(number)).FirstOrDefault();
            if (result is null) return false;
            else return true;
        }

        public bool CheckVINNumberExist(string number)
        {
            var result = _dbContext.Vehicles.Where(e => e.VIN_Number.Contains(number)).FirstOrDefault();
            if (result is null) return false;
            else return true;
        }
    }
}
