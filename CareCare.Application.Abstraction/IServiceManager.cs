using CareCare.Core.Application.Abstraction.Services.Auth;
using CareCare.Core.Application.Abstraction.Services.FeedBack;
using CareCare.Core.Application.Abstraction.Services.Vehicles;

namespace CareCare.Core.Application.Abstraction
{
    public interface IServiceManager
    {
        public IAuthService AuthService { get; }

        public IVehicleService VehicleService { get; }

        public IFeedBackService FeedBackService { get; }

    }
}
