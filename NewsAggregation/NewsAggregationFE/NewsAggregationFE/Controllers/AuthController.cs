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
            string email = _consoleView.ReadInput("Enter your email");
            string password = _consoleView.ReadInput("Enter your Password");
            return await _authService.Login(new LoginUserData() { Email = email, Password = password });
        }
    }
}
