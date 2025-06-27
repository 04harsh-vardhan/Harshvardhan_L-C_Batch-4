using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Services.Interfaces;
using NewsAggregationFE.State;
using NewsAggregationFE.Util;

namespace NewsAggregationFE.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _baseUrl;
        private readonly AppState _appState;

        public AuthService(AppState appState, AppConfiguration config)
        {
            _appState = appState;
            _baseUrl = config.ApiUrls.GetUserUrl();
        }

        public async Task<bool> Login(LoginUserData loginData)
        {
            try
            {
                LoginResponseDto response = await HttpRequest.PostRequest<LoginUserData, LoginResponseDto>(loginData, $"{_baseUrl}/login/user");
                if (response == null)
                {
                    throw new Exception("Login failed");
                }

                _appState.JwtToken = response.Token.ToString();
                _appState.Role = response.Role;
                _appState.Username = response.Username ?? "";
                _appState.UserId = response.UserId;
                HttpRequest.SetAuthToken(_appState.JwtToken);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed: {ex.Message}");
            }
        }

        public async Task<bool> SignUp(User user)
        {
            try
            {
                var response = await HttpRequest.PostRequest<User, dynamic>(user, $"{_baseUrl}/register");
                if (response == null)
                {
                    throw new Exception("Registration failed");
                }
                return true;

            }
            catch (Exception ex)
            {
                throw new Exception($"Registration failed: {ex.Message}");
            }
        }
    }
}
