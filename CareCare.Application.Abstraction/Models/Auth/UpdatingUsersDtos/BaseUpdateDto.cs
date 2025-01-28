namespace CareCare.Core.Application.Abstraction.Models.Auth.UpdatingUsersDtos
{
    public class BaseUpdateDto
    {
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
