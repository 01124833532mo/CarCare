using CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Roles;

namespace CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users
{
    public class UserRoleViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Type { get; set; }

        public IEnumerable<RoleDto> Roles { get; set; }
        public IEnumerable<string> RolesToReturn { get; set; }
    }
}
