using CareCare.Core.Application.Abstraction.Models.Vehicles;

namespace CareCare.Core.Application.Abstraction.Services.Vehicles
{
    public interface IVehicleService
    {
        Task<VehicleToReturn> CreateVehicle(CreateVehicleDto createVehicleDto);

        Task<string> DeleteVehicle(int id);


        Task<VehicleToReturn> GetVehicle(int id);


    }
}
