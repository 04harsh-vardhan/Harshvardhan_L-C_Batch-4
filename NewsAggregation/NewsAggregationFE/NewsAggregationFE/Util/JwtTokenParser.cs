using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NewsAggregationFE.Util
{
    public static class JwtTokenParser
    {
        public static (int? UserId, string? Username) ExtractUserInfo(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);
                
                // Based on backend JwtTokenGenerator.cs:
                // - Username is stored in "sub" claim
                // - UserId is stored in "nameid" claim
                var usernameClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == "sub");
                var userIdClaim = jsonToken.Claims.FirstOrDefault(x => x.Type == "nameid");
                
                int? userId = null;
                if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
                {
                    userId = parsedUserId;
                }
                
                string? username = usernameClaim?.Value;
                
                return (userId, username);
            }
            catch (Exception)
            {
                return (null, null);
            }
        }
        
        public static bool IsTokenValid(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadJwtToken(token);
                return jsonToken.ValidTo > DateTime.UtcNow;
            }
            catch
            {
                return false;
            }
        }
    }
}