using Microsoft.Extensions.Options;
using WorkOffice.Contracts.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WorkOffice.Web.Utilities
{
    public interface IJwtTokenGenerator
    {
        string CreateToken(AuthenticationResponse Input);
    }
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly JwtIssuerOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtIssuerOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string CreateToken(AuthenticationResponse model)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, model.Email),
                 new Claim("UserId", model.UserId.ToString()),
                  new Claim("UserName", model.FullName()),
                new Claim(JwtRegisteredClaimNames.Jti, _jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat,
                    new DateTimeOffset(_jwtOptions.IssuedAt).ToUnixTimeSeconds().ToString(),
                    ClaimValueTypes.Integer64)
            };
            var jwt = new JwtSecurityToken(
                _jwtOptions.Issuer,
                _jwtOptions.Audience,
                claims,
                _jwtOptions.NotBefore,
                _jwtOptions.Expiration,
                _jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}

