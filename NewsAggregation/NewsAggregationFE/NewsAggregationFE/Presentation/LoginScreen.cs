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

    }
}
