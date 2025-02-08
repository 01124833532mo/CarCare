using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Services.Auth;
using CareCare.Core.Application.Abstraction.Services.Contacts;
using CareCare.Core.Application.Abstraction.Services.FeedBack;
using CareCare.Core.Application.Abstraction.Services.ServiceTypes;
using CareCare.Core.Application.Abstraction.Services.Vehicles;

namespace CarCare.Core.Application
{

    public class ServiceManger : IServiceManager
    {
        private readonly Lazy<IAuthService> _authService;
        private readonly Lazy<IVehicleService> _vehicleService;
        private readonly Lazy<IFeedBackService> _feedBackService;
        private readonly Lazy<IServiceTypeService> _serviceTypeService;
        private readonly Lazy<IContactService> _contactService;

        public ServiceManger(Func<IAuthService> authfactory, Func<IVehicleService> vehiclefactory, Func<IFeedBackService> feedBackfactory, Func<IServiceTypeService> servicetypesfactory, Func<IContactService> contactService)
        {

            _authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _vehicleService = new Lazy<IVehicleService>(vehiclefactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _feedBackService = new Lazy<IFeedBackService>(feedBackfactory, LazyThreadSafetyMode.ExecutionAndPublication);
            _serviceTypeService = new Lazy<IServiceTypeService>(servicetypesfactory, LazyThreadSafetyMode.ExecutionAndPublication);
		      	_contactService = new Lazy<IContactService>(contactService, LazyThreadSafetyMode.ExecutionAndPublication);

        }


        public IAuthService AuthService => _authService.Value;

        public IVehicleService VehicleService => _vehicleService.Value;


        public IFeedBackService FeedBackService => _feedBackService.Value;

        public IServiceTypeService ServiceTypeService => _serviceTypeService.Value;
        
        public IContactService ContactService => _contactService.Value;
    }

}
