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
            _view.ShowMessages("Welcome to the News Aggregator application." +
                " Please choose the\r\noptions below." + "\n1. Login" + "\n2. Sign up" + "\n3. Exit");
            return _view.ReadInput("");
        }
    }
}
