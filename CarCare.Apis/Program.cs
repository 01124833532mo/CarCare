using CarCare.Apis.Extinsions;
using CarCare.Apis.Middlewares;
using CarCare.Core.Application;
using CarCare.Infrastructure;
using CarCare.Infrastructure.Persistence;
using CarCare.Shared.ErrorModoule.Errors;
using Microsoft.AspNetCore.Mvc;

namespace CarCare.Apis
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
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
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpContextAccessor();
            builder.Services.RegesteredPresestantLayer();
            builder.Services.AddApplicationServices(builder.Configuration);
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddIdentityServices(builder.Configuration);
            builder.Services.AddInfrastructureServices(builder.Configuration);


            var app = builder.Build();

            await app.InitializerCarCareIdentityContextAsync();

            // Configure the HTTP request pipeline.

            app.UseMiddleware<ExeptionHandlerMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStatusCodePagesWithReExecute("/Errors/{0}");


            app.UseStaticFiles();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
