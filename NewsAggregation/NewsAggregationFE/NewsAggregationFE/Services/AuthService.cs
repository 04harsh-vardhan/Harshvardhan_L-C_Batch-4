using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Services.Interfaces;
using NewsAggregationFE.State;
using NewsAggregationFE.Util;

namespace NewsAggregationFE.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _loginUrl = "https://localhost:7035/api/User/login/user";
        private AppState _appState;
        public AuthService(AppState appState)
        {
            _appState = appState;
        }
        public async Task<bool> Login(string email, string password)
        {
            LoginUserData loginUserData = new LoginUserData() { Email = email, Password = password };
            string? jwtToken = await HttpRequest.GetRequest<string>(_loginUrl);
            if (jwtToken == null)
            {
                return false;
            }
            _appState.JwtToken = jwtToken;
            return true;
        }
    }
}
