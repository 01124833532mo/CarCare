namespace CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Technicals
{
    public class TechRoleViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Type { get; set; }
        public required string Email { get; set; }
        public required string NationalId { get; set; }

        public IEnumerable<object> Roles { get; set; }
        //public IEnumerable<string> RolesToReturn { get; set; }
    }
}
