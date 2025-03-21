namespace CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos
{
    public class TechRegisterDto : BaseRegisterDto
    {




        public required string NationalId { get; set; }

        public int ServiceId { get; set; }


    }
}
