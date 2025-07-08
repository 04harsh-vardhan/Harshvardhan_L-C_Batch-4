using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services
{
    public class UserContext : IUserContext
    {
        public int? UserId { get; private set; }

        public void SetUserFromToken(string jwtToken)
        {
            try
            {
                if (string.IsNullOrEmpty(jwtToken))
                {
                    UserId = null;
                    return;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.ReadJwtToken(jwtToken);
                
                var userIdClaim = token.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
                {
                    UserId = userId;
                }
                else
                {
                    UserId = null;
                }
            }
            catch
            {
                UserId = null;
            }
        }
    }
}