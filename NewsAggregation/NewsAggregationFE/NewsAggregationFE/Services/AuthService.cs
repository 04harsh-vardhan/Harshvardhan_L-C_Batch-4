using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Services.Interfaces;
using NewsAggregationFE.State;
using NewsAggregationFE.Util;

namespace NewsAggregationFE.Services
{
    public class AuthService : IAuthService
    {
        private readonly string _baseUrl = "https://localhost:7035/api/User";
        private readonly AppState _appState;

        public AuthService(AppState appState)
        {
            _appState = appState;
        }

        public async Task<bool> Login(LoginUserData loginData)
        {
            try
            {
                LoginResponseDto response = await HttpRequest.GetPost<LoginUserData, LoginResponseDto>(loginData, $"{_baseUrl}/login/user");
                if (response == null)
                {
                    throw new Exception("Login failed");
                }

                _appState.JwtToken = response.Token.ToString();
                _appState.Role = response.Role;
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
                var response = await HttpRequest.GetPost<User, dynamic>(user, $"{_baseUrl}/register");
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
