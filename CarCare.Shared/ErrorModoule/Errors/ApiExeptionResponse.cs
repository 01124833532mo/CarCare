using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarCare.Shared.ErrorModoule.Errors
{
    public class ApiExeptionResponse : ApiResponse
    {

        public string? Details { get; set; }

        public ApiExeptionResponse(int statuscode, string? message = null, string? details = null)
            : base(statuscode, message)
        {
            Details = details;

        }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
