using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WorkOffice.Contracts.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WorkOffice.Web.Utilities
{
    public class GenerateTokenExtentions
    {
        private readonly IConfiguration configuration;

        public GenerateTokenExtentions(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public String GenerateToken(AuthenticationResponse Input)
        {
            try
            {
                //Set issued at date
                DateTime issuedAt = DateTime.UtcNow;
                //set the time when it expires
                int tokenDuration = int.Parse(configuration.GetSection("AppSetting:TokenExpires").Value);
                DateTime expires = DateTime.UtcNow.AddHours(tokenDuration);

                var tokenHandler = new JwtSecurityTokenHandler();

                //create a identity and add claims to the user which we want to log in
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", Input.UserId != Guid.Empty ? Input.UserId.ToString() : ""),
                    new Claim("UserName", Input.FullName() != null ? Input.FullName() : ""),
                    new Claim("Email", Input.Email != null ?  Input.Email : ""),
                });

                var now = DateTime.UtcNow;
                var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(configuration.GetSection("AppSetting:Token").Value));
                var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                //create the jwt
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    NotBefore = issuedAt,
                    Subject = claimsIdentity,
                    Expires = expires,
                    SigningCredentials = signingCredentials
                };

                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);

                var tokenString = tokenHandler.WriteToken(token);

                return tokenString;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
