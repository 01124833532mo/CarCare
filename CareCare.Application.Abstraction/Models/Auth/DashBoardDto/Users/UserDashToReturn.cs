namespace CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users
{
    public class UserDashToReturn
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Type { get; set; }
    }
}
