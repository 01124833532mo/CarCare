namespace CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users
{
    public class UserRoleViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Type { get; set; }

        public IEnumerable<object> Roles { get; set; }
    }
}
