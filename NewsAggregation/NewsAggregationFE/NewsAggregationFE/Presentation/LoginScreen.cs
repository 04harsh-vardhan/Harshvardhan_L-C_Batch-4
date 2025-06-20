using System.Text.RegularExpressions;
using NewsAggregationFE.Core.Interfaces;
using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Services.Interfaces;

namespace NewsAggregationFE.Presentation
{
    public class LoginScreen : ILoginScreen
    {
        private readonly IConsoleView _view;
        private readonly IAuthService _authService;

        public LoginScreen(IConsoleView view, IAuthService authService)
        {
            _view = view;
            _authService = authService;
        }

        public async Task<bool> Login()
        {
            _view.ShowMessages("\nLogin");
            var email = _view.ReadInput("Enter email: ");
            var password = _view.ReadInput("Enter password: ");

            try
            {
                await _authService.Login(new LoginUserData { Email = email, Password = password });
                _view.ShowMessages($"\nWelcome !");
                return true;
            }
            catch (Exception ex)
            {
                _view.ShowMessages($"\nError: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SignUp()
        {
            _view.ShowMessages("\nSign Up");
            var username = _view.ReadInput("Enter username: ");
            var email = _view.ReadInput("Enter email: ");
            var password = _view.ReadInput("Enter password: ");

            if (!ValidateEmail(email))
            {
                _view.ShowMessages("\nError: Invalid email format");
                return false;
            }

            try
            {
                return await _authService.SignUp(new User
                {
                    Username = username,
                    Email = email,
                    Password = password,
                    Role = UserRole.User
                });
            }
            catch (Exception ex)
            {
                _view.ShowMessages($"\nError: {ex.Message}");
                return false;
            }
        }

        private bool ValidateEmail(string email)
        {
            var pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
