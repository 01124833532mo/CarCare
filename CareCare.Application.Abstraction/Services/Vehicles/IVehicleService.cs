using CareCare.Core.Application.Abstraction.Common;
using CareCare.Core.Application.Abstraction.Models.Vehicles;
using System.Security.Claims;

namespace CareCare.Core.Application.Abstraction.Services.Vehicles
{
    public interface IVehicleService
    {
        Task<VehicleToReturn> CreateVehicle(CreateVehicleDto createVehicleDto);

        Task<string> DeleteVehicle(int id);


        Task<VehicleToReturn> GetVehicle(int id);

        Task<Pagination<VehicleToReturn>> GetAllVehicles(SpecParams specParams);
        Task<Pagination<VehicleToReturn>> GetAllVehicleForUser(ClaimsPrincipal claimsPrincipal, SpecParams specParams);


    }
}
