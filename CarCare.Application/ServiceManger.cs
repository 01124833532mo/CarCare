using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Services.Auth;
using CareCare.Core.Application.Abstraction.Services.FeedBack;
using CareCare.Core.Application.Abstraction.Services.Vehicles;

namespace CarCare.Core.Application
{
	public class ServiceManger : IServiceManager
	{
		private readonly Lazy<IAuthService> _authService;
		private readonly Lazy<IVehicleService> _vehicleService;
		private readonly Lazy<IFeedBackService> _feedBackService;

		public ServiceManger(Func<IAuthService> authfactory, Func<IVehicleService> vehiclefactory, Func<IFeedBackService> feedBackfactory)
		{

			_authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);
			_vehicleService = new Lazy<IVehicleService>(vehiclefactory, LazyThreadSafetyMode.ExecutionAndPublication);
			_feedBackService = new Lazy<IFeedBackService>(feedBackfactory, LazyThreadSafetyMode.ExecutionAndPublication);

		}

		public IAuthService AuthService => _authService.Value;

		public IVehicleService VehicleService => _vehicleService.Value;

		public IFeedBackService FeedBackService => _feedBackService.Value;
	}
}
