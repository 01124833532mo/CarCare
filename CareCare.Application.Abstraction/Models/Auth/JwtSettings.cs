﻿namespace CareCare.Core.Application.Abstraction.Models.Auth
{
    public class JwtSettings
    {
        public required string Key { get; set; }
        public required string Audience { get; set; }
        public required string Issuer { get; set; }
        public required double DurationInMinutes { get; set; }
        public required int JWTRefreshTokenExpire { get; set; }


    }
}
