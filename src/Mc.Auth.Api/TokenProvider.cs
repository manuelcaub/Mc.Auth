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
        private readonly AuthorizationSettings _authSettings;

        public TokenProvider(AuthorizationSettings authSettings)
        {
            _authSettings = authSettings;
        }

        public string GetToken()
        {
            var jwsToken = new JwtSecurityToken(_authSettings.Issuer,
                _authSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(_authSettings.Expires),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.SecretKey)), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwsToken);
        }
    }
}
