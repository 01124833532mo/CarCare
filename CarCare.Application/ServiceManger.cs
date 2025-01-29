using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Services.Auth;
using CareCare.Core.Application.Abstraction.Services.Vehicles;

namespace CarCare.Core.Application
{
    public class ServiceManger : IServiceManager
    {
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IVehicleService> _vehicleService;


        public ServiceManger(Func<IAuthService> authfactory, Func<IVehicleService> vehiclefactory)
        {

            _authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _vehicleService = new Lazy<IVehicleService>(vehiclefactory, LazyThreadSafetyMode.ExecutionAndPublication);


        }

        public IAuthService AuthService => _authService.Value;

        public IVehicleService VehicleService => _vehicleService.Value;
    }
}
