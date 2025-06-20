using NewsAggregationFE.Core.Interfaces;
using NewsAggregationFE.Core.Models;

namespace NewsAggregationFE.Presentation
{
    public class AdminScreen : IAdminScreen
    {
        private readonly IConsoleView _view;
        private readonly User _currentUser;

        public AdminScreen(IConsoleView view, User currentUser)
        {
            _view = view;
            _currentUser = currentUser;
        }

        public async Task<bool> ShowMenu()
        {
            while (true)
            {
                _view.ShowMessages($"\nWelcome to the News Aggregator application, {_currentUser.Username}! Date: {DateTime.Now:dd-MMM-yyyy} Time:{DateTime.Now:h:mmtt}");
                _view.ShowMessages("\n1. View the list of external servers and status");
                _view.ShowMessages("2. View the external server's details");
                _view.ShowMessages("3. Update/Edit the external server's details");
                _view.ShowMessages("4. Add new News Category");
                _view.ShowMessages("5. Logout");

                var choice = _view.ReadInput("\nEnter your choice: ");

                switch (choice)
                {
                    case "1":
                        await ShowExternalServers();
                        break;
                    case "2":
                        await ShowServerDetails();
                        break;
                    case "3":
                        await UpdateServerDetails();
                        break;
                    case "4":
                        await AddNewsCategory();
                        break;
                    case "5":
                        return false;
                    default:
                        _view.ShowMessages("\nInvalid choice. Please try again.");
                        break;
                }
            }
        }

        public async Task ShowExternalServers()
        {
            _view.ShowMessages("\nList of external servers:");
            _view.ShowMessages("1. News API - Active - last accessed: " + DateTime.Now.ToString("dd MMM yyyy"));
            _view.ShowMessages("2. The News API - Active - last accessed: " + DateTime.Now.ToString("dd MMM yyyy"));
            await Task.CompletedTask;
        }

        public async Task ShowServerDetails()
        {
            _view.ShowMessages("\nList of external server details:");
            _view.ShowMessages("1. News API - <API KEY>");
            _view.ShowMessages("2. The News API - <API KEY>");
            await Task.CompletedTask;
        }

        public async Task UpdateServerDetails()
        {
            _view.ShowMessages("\nUpdate/Edit the external server's details");
            var serverId = _view.ReadInput("Enter the external server ID: ");
            var apiKey = _view.ReadInput("Enter the updated API key: ");
            _view.ShowMessages("\nAPI key updated successfully!");
            await Task.CompletedTask;
        }

        public async Task AddNewsCategory()
        {
            _view.ShowMessages("\nAdd new News Category");
            var category = _view.ReadInput("Enter the new category name: ");
            _view.ShowMessages("\nCategory added successfully!");
            await Task.CompletedTask;
        }
    }
} 