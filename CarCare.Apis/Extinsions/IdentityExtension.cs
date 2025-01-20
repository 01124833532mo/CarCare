using CarCare.Core.Application.Services;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Infrastructure.Persistence._Identity;
using CareCare.Core.Application.Abstraction.Models.Auth;
using CareCare.Core.Application.Abstraction.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace CarCare.Apis.Extinsions
{
    public static class IdentityExtension
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddIdentity<ApplicationUser, IdentityRole>((identityOptions) =>
            {
                //identityOptions.SignIn.RequireConfirmedPhoneNumber = true;
                identityOptions.Password.RequireDigit = true;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Lockout.AllowedForNewUsers = true;
                identityOptions.Lockout.MaxFailedAccessAttempts = 5;
                identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(5);
            })
                .AddEntityFrameworkStores<CarCarIdentityDbContext>();

            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(Func<IAuthService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IAuthService>();
            });

            services.AddAuthentication((configurationOptions) =>
            {
                configurationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                configurationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
                .AddJwtBearer((configurationOptions) =>
                {
                    configurationOptions.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,


                        ClockSkew = TimeSpan.FromHours(0),
                        ValidAudience = configuration["JwtSettings:Audience"],
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                    };
                });

            return services;
        }
    }
}
