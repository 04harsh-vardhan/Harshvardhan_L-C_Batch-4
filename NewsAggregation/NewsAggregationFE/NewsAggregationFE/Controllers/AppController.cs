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
                try
                {
                    switch (choice)
                    {
                        case "0":
                            choice = _welcomeScreen.ShowWelcomeMenu();
                            break;
                        case "1":
                            bool loginSuccess = await _authController.Login();
                            if (loginSuccess)
                            {
                                if (_appState.Role?.ToLower() == "user")
                                {
                                    bool continueUserSession = true;
                                    while (continueUserSession)
                                    {
                                        continueUserSession = await _userController.UserMenu();
                                    }
                                    choice = "0";
                                }
                                else if (_appState.Role?.ToLower() == "admin")
                                {
                                    bool continueAdminSession = true;
                                    while (continueAdminSession)
                                    {
                                        continueAdminSession = await _adminController.AdminMenu();
                                    }
                                    choice = "0";
                                }
                                else
                                {
                                    Console.WriteLine("Unknown user role. Returning to main menu.");
                                    choice = "0";
                                }
                            }
                            else
                            {
                                choice = "0";
                            }
                            break;
                        case "2":
                            bool signupSuccess = await _authController.SignUp();
                            if (signupSuccess)
                            {
                                Console.WriteLine("Registration completed successfully. Please login to continue.");
                            }
                            choice = "0";
                            break;
                        case "3":
                            Console.WriteLine("Goodbye!");
                            running = false;
                            break;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            choice = "0";
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    choice = "0";
                }
            }
        }
    }
}
