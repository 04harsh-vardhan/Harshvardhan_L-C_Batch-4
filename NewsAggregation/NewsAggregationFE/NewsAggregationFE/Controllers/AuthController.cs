using NewsAggregationFE.Controllers.Interfaces;
using NewsAggregationFE.Core.Interfaces;
using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Services.Interfaces;

namespace NewsAggregationFE.Controllers
{
    public class AuthController : IAuthController
    {
        private readonly IConsoleView _consoleView;
        private readonly IAuthService _authService;
        public AuthController(IConsoleView consoleView, IAuthService authService)
        {
            _consoleView = consoleView;
            _authService = authService;
        }
        public async Task<bool> Login()
        {
            string email = _consoleView.ReadInput("Enter your email:");
            string password = _consoleView.ReadInput("Enter your password:");
            return await _authService.Login(new LoginUserData() { Email = email, Password = password });
        }

        public async Task<bool> SignUp()
        {
            try
            {
                _consoleView.ShowMessages("\n--- User Registration ---");

                string username = _consoleView.ReadInput("Enter your username:");
                string email = _consoleView.ReadInput("Enter your email:");
                string password = _consoleView.ReadInput("Enter your password:");
                string confirmPassword = _consoleView.ReadInput("Confirm your password:");

                if (password != confirmPassword)
                {
                    _consoleView.ShowMessages("Passwords do not match.");
                    return false;
                }

                var signupData = new SignupUserDto
                {
                    UserName = username,
                    Email = email,
                    Password = password,
                    RoleId = 2
                };

                bool success = await _authService.SignUp(signupData);
                if (success)
                {
                    _consoleView.ShowMessages("Registration successful! You can now login.");
                    return true;
                }
                else
                {
                    _consoleView.ShowMessages("Registration failed. Please try again.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Registration failed: {ex.Message}");
                return false;
            }
        }
    }
}
