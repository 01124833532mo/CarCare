using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Application
{
    public class ServiceManger : IServiceManager
    {
        private readonly Lazy <IAuthService> _authService;


        public ServiceManger(Func<IAuthService> authfactory)
        {

            _authService = new Lazy<IAuthService>(authfactory, LazyThreadSafetyMode.ExecutionAndPublication);


        }

        public IAuthService AuthService => _authService.Value;
    }
}
