using NewsAggregationFE.Core.Interfaces;

namespace NewsAggregationFE.Presentation
{
    public class WelcomeScreen : IWelcomeScreen
    {
        private readonly IConsoleView _view;
        public WelcomeScreen(IConsoleView view)
        {
            _view = view;
        }
        public string ShowWelcomeMenu()
        {
            _view.ShowMessages("\n" + new string('=', 50));
            _view.ShowMessages("       WELCOME TO NEWS AGGREGATOR");
            _view.ShowMessages(new string('=', 50));
            _view.ShowMessages("\nPlease choose an option:");
            _view.ShowMessages("1. Login");
            _view.ShowMessages("2. Sign up (Not implemented)");
            _view.ShowMessages("3. Exit");
            _view.ShowMessages(new string('-', 30));
            return _view.ReadInput("Enter your choice:");
        }
    }
}
