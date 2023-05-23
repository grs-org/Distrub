using System;

namespace Distribution.Shared.Response
{
    public class TokenResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public string Message { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}