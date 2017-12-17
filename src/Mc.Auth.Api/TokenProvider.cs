using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Mc.Auth.Core.Entities;
using Mc.Auth.Core.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Mc.Auth.Api
{
    public class TokenProvider : ITokenProvider
    {
        private readonly Authentication _authSettings;
        private readonly JwtSecurityTokenHandler _securityTokenHandler;

        public TokenProvider(Authentication authSettings, JwtSecurityTokenHandler securityTokenHandler)
        {
            _authSettings = authSettings;
            _securityTokenHandler = securityTokenHandler;
        }

        public string GetToken()
        {
            var jwsToken = new JwtSecurityToken(_authSettings.Issuer,
                _authSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_authSettings.Expires),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SecretKey)), SecurityAlgorithms.HmacSha256));

            return _securityTokenHandler.WriteToken(jwsToken);
        }
    }
}
