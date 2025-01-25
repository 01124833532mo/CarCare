using CarCare.Shared.ErrorModoule.Errors;
using CarCare.Shared.ErrorModoule.Exeptions;
using System.Net;

namespace CarCare.Apis.Middlewares
{
    public class ExeptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExeptionHandlerMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExeptionHandlerMiddleware(RequestDelegate next, ILogger<ExeptionHandlerMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {


            try
            {

                await _next(httpContext);

                if (httpContext.Response.StatusCode == (int)HttpStatusCode.MethodNotAllowed)
                {
                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    httpContext.Response.ContentType = "application/json";
                    var respnse = new ApiResponse((int)HttpStatusCode.Unauthorized, $"You Are Not Authorized");
                    await httpContext.Response.WriteAsync(respnse.ToString());
                }

            }
            catch (Exception ex)
            {
                if (_env.IsDevelopment())
                {

                    //development mode

                    _logger.LogError(ex, ex.Message);

                }

                else
                {
                    // production mode
                    // log exeption details t (file | text)

                }


                await HandleExeptionAsync(httpContext, ex);
            }




        }

        private async Task HandleExeptionAsync(HttpContext httpContext, Exception ex)
        {
            ApiResponse response;

            switch (ex)
            {
                case NotFoundExeption:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(404, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());

                    break;

                case ValidationExeption validationExeption:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiValidationErrorResponse(ex.Message) { Errors = validationExeption.Errors };
                    await httpContext.Response.WriteAsync(response.ToString());

                    break;


                case BadRequestExeption:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(400, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());

                    break;

                case UnAuthorizedExeption:

                    httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    httpContext.Response.ContentType = "application/json";
                    response = new ApiResponse(401, ex.Message);
                    await httpContext.Response.WriteAsync(response.ToString());

                    break;

                default:
                    response = _env.IsDevelopment() ? new ApiExeptionResponse((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace?.ToString())
                        : new ApiExeptionResponse((int)HttpStatusCode.InternalServerError, ex.Message);

                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    httpContext.Response.ContentType = "application/json";

                    await httpContext.Response.WriteAsync(response.ToString());


                    break;
            }

        }
    }
}
