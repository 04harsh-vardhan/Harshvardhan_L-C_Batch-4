using NewsAggregation.Models;

namespace NewsAggregation.Utils
{
    public interface IJwtTokenGenerator
    {
        public string GenerateJwtToken<T>(T user, string role) where T : User;
    }
}
