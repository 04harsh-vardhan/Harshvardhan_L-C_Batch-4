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
                
                // Extract userId and username from JWT token
                var (userId, username) = JwtTokenParser.ExtractUserInfo(response.Token);
                _appState.UserId = userId;
                _appState.Username = username ?? "";
                
                HttpRequest.SetAuthToken(_appState.JwtToken);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Login failed: {ex.Message}");
            }
        }

        public async Task<bool> SignUp(SignupUserDto signupData)
        {
            try
            {
                var response = await HttpRequest.PostRequest<SignupUserDto, bool>(signupData, $"{_baseUrl}/signup");
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception($"Registration failed: {ex.Message}", ex);
            }
        }
    }
}
