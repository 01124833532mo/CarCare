﻿using CarCare.Infrastructure.AttachementService;
using CarCare.Infrastructure.Cache_Sevice;
using CarCare.Infrastructure.Payment_Service;
using CarCare.Shared.Models;
using CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace CarCare.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(IAttachmentService), typeof(AttachmentService));

            // Register IMemoryCache
            services.AddMemoryCache();

            // Register Redis connection (handle errors gracefully)
            services.AddSingleton<IConnectionMultiplexer>(_ =>
            {
                var connectionString = configuration.GetConnectionString("Redis");
                try
                {
                    return ConnectionMultiplexer.Connect(connectionString!);
                }
                catch
                {
                    // Return a dummy connection multiplexer if Redis is unavailable
                    return ConnectionMultiplexer.Connect("localhost:6379,abortConnect=false");
                }
            });

            // Register ResponseCacheService
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            services.AddScoped(typeof(IPaymentService), typeof(PaymentService));

            services.Configure<StripSettings>(configuration.GetSection("StripeSettings"));

            return services;

        }
    }
}
