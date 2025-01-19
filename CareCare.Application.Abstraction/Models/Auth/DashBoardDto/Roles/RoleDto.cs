namespace CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles
{
    public class RoleDto
    {

        public string Id { get; set; }

        public required string Name { get; set; }

        public bool IsSelected { get; set; }

        public RoleDto()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
