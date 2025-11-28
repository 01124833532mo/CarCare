using CarCare.Shared.ErrorModoule.Errors;
using LinkDev.Talabat.APIs.Services;
using LinkDev.Talabat.Core.Application.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis.Extinsions
{
    public static class DependencyInjection
    {

        public static IServiceCollection RegesteredPresestantLayer(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));


            #region Swagger
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            #endregion

            services.AddHttpContextAccessor();


            services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
                options.InvalidModelStateResponseFactory = (actionContext =>
                {
                    var Errors = actionContext.ModelState.Where(e => e.Value!.Errors.Count() > 0)
                                                .SelectMany(e => e.Value!.Errors).Select(e => e.ErrorMessage);

                    return new BadRequestObjectResult(new ApiValidationErrorResponse() { Errors = Errors });

                });
            }
            ).AddApplicationPart(typeof(Controllers.AssemblyInformation).Assembly);


            #region CORS
            // To Do Allow SpecificOrigins
            services.AddCors(corsOptions =>
              {
                  corsOptions.AddPolicy("TalabatPolicy", policyBuilder =>
                  {
                      policyBuilder.AllowAnyHeader()
                                   .AllowAnyMethod()
                                   .AllowAnyOrigin(); // Allow all domains
                  });
              });

            #endregion

            return services;
        }

    }
}
