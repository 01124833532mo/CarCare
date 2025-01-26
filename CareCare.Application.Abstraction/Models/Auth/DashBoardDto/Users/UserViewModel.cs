namespace CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users
{
    public class UserViewModel
    {
        public required string Id { get; set; }
        public required string UserName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }

        public required string Type { get; set; }

        public IEnumerable<string> Roles { get; set; }

    }
}
