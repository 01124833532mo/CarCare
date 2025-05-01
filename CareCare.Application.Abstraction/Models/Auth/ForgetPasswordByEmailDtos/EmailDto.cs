namespace CareCare.Core.Application.Abstraction.Models.Auth.ForgetPasswordByEmailDtos
{
    public class EmailDto
    {

        public required string To { get; set; }
        public required string Subject { get; set; }
        public required string Body { get; set; }
        public bool IsBodyHtml { get; set; } = true;



    }
}
