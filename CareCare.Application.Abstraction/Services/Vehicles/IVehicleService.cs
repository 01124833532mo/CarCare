using CareCare.Core.Application.Abstraction.Models.Vehicles;

namespace CareCare.Core.Application.Abstraction.Services.Vehicles
{
    public interface IVehicleService
    {
        Task<CreateVehicleToReturn> CreateVehicle(CreateVehicleDto createVehicleDto);


    }
}
