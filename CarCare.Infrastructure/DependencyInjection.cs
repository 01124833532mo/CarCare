using CarCare.Infrastructure.AttachementService;
using CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IAttachmentService), typeof(AttachmentService));
            return services;
        }
    }
}
