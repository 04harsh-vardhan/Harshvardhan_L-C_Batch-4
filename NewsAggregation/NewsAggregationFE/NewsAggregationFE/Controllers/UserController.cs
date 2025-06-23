using NewsAggregationFE.Controllers.Interfaces;
using NewsAggregationFE.Core.Interfaces;
using NewsAggregationFE.Services.Interfaces;
using NewsAggregationFE.State;

namespace NewsAggregationFE.Controllers
{
    public class UserController : IUserController
    {
        private readonly IConsoleView _consoleView;
        private readonly IUserService _userService;
        private readonly AppState _appState;
        public UserController(IConsoleView consoleView, AppState appState, IUserService userService)
        {
            _consoleView = consoleView;
            _appState = appState;
            _userService = userService;
        }
        public async Task<bool> UserMenu()
        {
            string choice = WelcomeMsg();
            switch (choice)
            {
                case "1":
                    await HeadLinesSubMenu();
                    break;
                case "2":
                    break;
                case "3":
                    break;
                case "4":
                    break;
                case "5":
                    break;
                default:
                    return false;
            }
        }
        private string WelcomeMsg()
        {
            _consoleView.ShowMessages($"Welcome to the News Application, {_appState.Username}! Date: {DateTime.Now.Date}\r\nTime:{DateTime.Now.TimeOfDay}");
            string userInput = _consoleView.ReadInput("Please choose the options below\r\n1. Headlines\r\n2. Saved Articles\r\n3. Search\r\n4. Notifications\r\n5. Logout");
            return userInput;
        }
        private async Task HeadLinesSubMenu()
        {
            string choice = _consoleView.ReadInput("Please choose the options below\r\n1. Today\r\n2. Date range\r\n3. Logout");
            switch (choice)
            {
                case "1":

            }
        }
    }
}
