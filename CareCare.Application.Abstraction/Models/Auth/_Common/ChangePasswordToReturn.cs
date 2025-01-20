namespace CareCare.Core.Application.Abstraction.Models.Auth._Common
{
    public class ChangePasswordToReturn
    {
        public required string Message { get; set; }
        public required string Token { get; set; }

    }
}
