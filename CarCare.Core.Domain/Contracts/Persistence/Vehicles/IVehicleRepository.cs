using CarCare.Core.Domain.Entities.Vehicles;

namespace CarCare.Core.Domain.Contracts.Persistence.Vehicles
{
    public interface IVehicleRepository : IGenericRepository<Vehicle, int>
    {
        bool CheckPlateNumberExist(string number);

        bool CheckVINNumberExist(string number);

    }
}
