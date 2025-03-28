using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CareCare.Core.Application.Abstraction
{
    public static class DependencyInjection
    {

        public static IServiceCollection RegisterApplicationOfAbstraction(this IServiceCollection services)
        {
            services
                .AddFluentValidationAutoValidation() // This should now be recognized
                .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
