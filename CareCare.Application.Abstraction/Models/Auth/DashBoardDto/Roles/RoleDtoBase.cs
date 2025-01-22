using System.Text.Json.Serialization;

namespace CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles
{
    public class RoleDtoBase
    {
        [JsonIgnore]
        public string Id { get; set; }

        public required string Name { get; set; }
        public RoleDtoBase()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
