using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarCare.Shared.ErrorModoule.Errors
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public required IEnumerable<string> Errors { get; set; }

        public ApiValidationErrorResponse(string? message = null) : base(400, message)
        {

        }

        public override string ToString()
        {
            return JsonSerializer.Serialize(new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }
    }
}
