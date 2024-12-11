using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarCare.Shared.ErrorModoule.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public ApiResponse(int statuscode, string? message = null)
        {
            StatusCode = statuscode;
            Message = message ?? GetDefaultMessageForStatusCode(statuscode);

        }

        private string? GetDefaultMessageForStatusCode(int statuscode)
        {

            return statuscode switch
            {
                400 => "A Bad Request ,you have made",
                401 => "Authorized , you are not",
                404 => "Resource was not Found",
                500 => "Errors are the path to the dark side , Errors lead to anger .Anger lead to hate . Hate lead to career change ",
                _ => null
            };

        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

    }
}
