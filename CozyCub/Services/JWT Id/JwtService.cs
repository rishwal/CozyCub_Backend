using CozyCub.JWT_Id;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CozyCub.Services.JWT_Id
{
    public class JwtService : IJwtService
    {


        private readonly string secretKey;

        public JwtService(IConfiguration configuration)
        {
            secretKey = configuration["Jwt:SecretKey"];
        }

        public int GetUserIdFromToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = Encoding.UTF8.GetBytes(secretKey);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(securityKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken)
                {
                    throw new SecurityTokenException("Invalid Jwt Tokwn.");
                }

                var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier);


                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                {
                    throw new SecurityTokenException("Invalid or missing user Id claim.");
                }

                return userId;

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error while fetching id from token {ex.Message}");
                return 0;
            }
        }
    }
}
