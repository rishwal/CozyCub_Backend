using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CozyCub.Helpers
{
    public class JwtHelper
    {
        //Function for Generating and returning token as a string.
        public static string GenerateJwtToken(string secretKey, string isuuer, string audience, ClaimsIdentity identity, int expixesMinutes = 30)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredtentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = isuuer,
                Audience = audience,
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(expixesMinutes),
                SigningCredentials = signingCredtentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
    }
}
