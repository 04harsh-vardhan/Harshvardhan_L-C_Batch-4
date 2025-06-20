using NewsAggregationFE.Core.Models;

namespace NewsAggregationFE.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<bool> Login(LoginUserData loginData);
        public Task<bool> SignUp(User user);
    }
}
