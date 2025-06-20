using NewsAggregationFE.Controllers.Interfaces;
using NewsAggregationFE.Core.Interfaces;
using NewsAggregationFE.State;

namespace NewsAggregationFE.Controllers
{
    public class AppController
    {
        private readonly IWelcomeScreen _welcomeScreen;
        private readonly IAuthController _authController;
        private readonly IAdminController _adminController;
        private readonly IUserController _userController;
        private readonly AppState _appState;
        public AppController(IWelcomeScreen welcomeScreen, IAuthController authController, IAdminController adminController, IUserController userController, AppState appState)
        {
            _welcomeScreen = welcomeScreen;
            _authController = authController;
            _adminController = adminController;
            _userController = userController;
            _appState = appState;
        }
        public async Task Run()
        {
            bool running = true;
            string choice = "0";
            while (running)
            {
                switch (choice)
                {
                    case "0":
                        choice = _welcomeScreen.ShowWelcomeMenu();
                        break;
                    case "1":
                        try
                        {
                            bool isSuccess = await _authController.Login();
                            if (!isSuccess)
                            {
                                choice = "0";
                            }
                            if (_appState.Role == "user")
                            {

                            }
                            else
                            {
                                await _adminController.AdminMenu();
                            }
                        }
                        catch (Exception ex)
                        {
                            choice = "0";
                        }
                        break;
                    case "2":
                        break;
                    case "3":
                        running = false;
                        break;
                }
            }
        }
    }
}
