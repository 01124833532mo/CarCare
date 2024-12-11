
using CarCare.Apis.Controllers;
using CarCare.Apis.Middlewares;
using CarCare.Shared.ErrorModoule.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CarCare.Apis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = false;
                options.InvalidModelStateResponseFactory = (actionContext =>
                {
                 var Errors=   actionContext.ModelState.Where(e => e.Value!.Errors.Count() > 0)
                                             .SelectMany(e => e.Value!.Errors).Select(e=>e.ErrorMessage);

                    return new BadRequestObjectResult(new ApiValidationErrorResponse() { Errors = Errors });
                                               
                });
            }
            ).AddApplicationPart(typeof(AssemblyInformation).Assembly);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
