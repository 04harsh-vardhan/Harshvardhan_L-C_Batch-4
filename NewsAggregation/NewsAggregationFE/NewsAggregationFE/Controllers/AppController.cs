using NewsAggregationFE.Controllers.Interfaces;
using NewsAggregationFE.Core.Interfaces;

namespace NewsAggregationFE.Controllers
{
    public class AppController
    {
        private readonly IWelcomeScreen _welcomeScreen;
        private readonly IAuthController _authController;
        public AppController(IWelcomeScreen welcomeScreen)
        {
            _welcomeScreen = welcomeScreen;
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
                        await _authController.Login();
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
