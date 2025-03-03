namespace CareCare.Core.Application.Abstraction.Models.Auth._Common
{
    public class RefreshDto
    {
        public required string Token { get; set; }

        public required string RefreshToken { get; set; }
    }
}
