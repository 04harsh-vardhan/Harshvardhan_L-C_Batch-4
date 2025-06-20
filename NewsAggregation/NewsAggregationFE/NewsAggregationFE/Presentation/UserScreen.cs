using NewsAggregationFE.Core.Interfaces;
using NewsAggregationFE.Core.Models;

namespace NewsAggregationFE.Presentation
{
    public class UserScreen : IUserScreen
    {
        private readonly IConsoleView _view;
        private readonly User _currentUser;

        public UserScreen(IConsoleView view, User currentUser)
        {
            _view = view;
            _currentUser = currentUser;
        }

        public async Task<bool> ShowMenu()
        {
            while (true)
            {
                _view.ShowMessages($"\nWelcome to the News Application, {_currentUser.Username}! Date: {DateTime.Now:dd-MMM-yyyy} Time:{DateTime.Now:h:mmtt}");
                _view.ShowMessages("\nPlease choose the options below");
                _view.ShowMessages("1. Headlines");
                _view.ShowMessages("2. Saved Articles");
                _view.ShowMessages("3. Search");
                _view.ShowMessages("4. Notifications");
                _view.ShowMessages("5. Logout");

                var choice = _view.ReadInput("\nEnter your choice: ");

                switch (choice)
                {
                    case "1":
                        await ShowHeadlinesMenu();
                        break;
                    case "2":
                        await ShowSavedArticles();
                        break;
                    case "3":
                        await ShowSearchMenu();
                        break;
                    case "4":
                        await ShowNotificationsMenu();
                        break;
                    case "5":
                        return false;
                    default:
                        _view.ShowMessages("\nInvalid choice. Please try again.");
                        break;
                }
            }
        }

        private async Task ShowHeadlinesMenu()
        {
            _view.ShowMessages("\n1. Today");
            _view.ShowMessages("2. Date range");
            _view.ShowMessages("3. Back");

            var choice = _view.ReadInput("\nEnter your choice: ");

            switch (choice)
            {
                case "1":
                    await ShowHeadlines();
                    break;
                case "2":
                    await ShowDateRangeHeadlines();
                    break;
                case "3":
                    return;
                default:
                    _view.ShowMessages("\nInvalid choice. Please try again.");
                    break;
            }
        }

        public async Task ShowHeadlines(DateTime? startDate = null, DateTime? endDate = null)
        {
            _view.ShowMessages("\nPlease choose the options below for Headlines");
            _view.ShowMessages("1. All");
            _view.ShowMessages("2. Business");
            _view.ShowMessages("3. Entertainment");
            _view.ShowMessages("4. Sports");
            _view.ShowMessages("5. Technology");

            var choice = _view.ReadInput("\nEnter your choice: ");
            var category = choice switch
            {
                "2" => "Business",
                "3" => "Entertainment",
                "4" => "Sports",
                "5" => "Technology",
                _ => "All"
            };

            // Mock article for demonstration
            var article = new Article
            {
                Id = "123",
                Title = "Tesla Unusual Options Activity - Tesla (NASDAQ: TSLA)",
                Description = "Deep-pocketed investors have adopted a bearish approach towards Tesla TSLA, and it's something market players shouldn't ignore. Our tracking of public options r…",
                Source = "benzinga.com",
                Url = "https://www.benzinga.com/insights/options/25/03/44379781/tesla-unusual-options-activity",
                Category = "business"
            };

            DisplayArticle(article);
            await ShowArticleOptions(article.Id);
        }

        private async Task ShowDateRangeHeadlines()
        {
            var startDate = _view.ReadInput("\nEnter start date (dd/MM/yyyy): ");
            var endDate = _view.ReadInput("Enter end date (dd/MM/yyyy): ");

            if (DateTime.TryParse(startDate, out var start) && DateTime.TryParse(endDate, out var end))
            {
                await ShowHeadlines(start, end);
            }
            else
            {
                _view.ShowMessages("\nInvalid date format. Please try again.");
            }
        }

        public async Task ShowSavedArticles()
        {
            _view.ShowMessages("\nS A V E D");
            
            // Mock saved article for demonstration
            var article = new Article
            {
                Id = "123",
                Title = "Tesla Unusual Options Activity - Tesla (NASDAQ: TSLA)",
                Description = "Deep-pocketed investors have adopted a bearish approach towards Tesla TSLA, and it's something market players shouldn't ignore. Our tracking of public options r…",
                Source = "benzinga.com",
                Url = "https://www.benzinga.com/insights/options/25/03/44379781/tesla-unusual-options-activity",
                Category = "business"
            };

            DisplayArticle(article);

            _view.ShowMessages("\n1. Back");
            _view.ShowMessages("2. Logout");
            _view.ShowMessages("3. Delete Article");

            var choice = _view.ReadInput("\nEnter your choice: ");

            switch (choice)
            {
                case "3":
                    var articleId = _view.ReadInput("\nEnter article ID to delete: ");
                    await DeleteSavedArticle(articleId);
                    break;
            }
        }

        private async Task ShowSearchMenu()
        {
            var query = _view.ReadInput("\nEnter search query: ");
            var startDate = _view.ReadInput("Enter start date (dd/MM/yyyy) or press Enter to skip: ");
            var endDate = _view.ReadInput("Enter end date (dd/MM/yyyy) or press Enter to skip: ");

            DateTime? start = null;
            DateTime? end = null;

            if (!string.IsNullOrEmpty(startDate) && DateTime.TryParse(startDate, out var parsedStart))
            {
                start = parsedStart;
            }

            if (!string.IsNullOrEmpty(endDate) && DateTime.TryParse(endDate, out var parsedEnd))
            {
                end = parsedEnd;
            }

            await SearchArticles(query, start, end);
        }

        public async Task SearchArticles(string query, DateTime? startDate = null, DateTime? endDate = null)
        {
            _view.ShowMessages($"\nS E A R C H");
            _view.ShowMessages($"Results for \"{query}\"");

            // Mock search result for demonstration
            var article = new Article
            {
                Id = "123",
                Title = "Tesla Unusual Options Activity - Tesla (NASDAQ: TSLA)",
                Description = "Deep-pocketed investors have adopted a bearish approach towards Tesla TSLA, and it's something market players shouldn't ignore. Our tracking of public options r…",
                Source = "benzinga.com",
                Url = "https://www.benzinga.com/insights/options/25/03/44379781/tesla-unusual-options-activity",
                Category = "business"
            };

            DisplayArticle(article);
            await ShowArticleOptions(article.Id);
        }

        private async Task ShowNotificationsMenu()
        {
            _view.ShowMessages("\nN O T I F I C A T I O N S");
            _view.ShowMessages("1. View Notifications");
            _view.ShowMessages("2. Configure Notifications");
            _view.ShowMessages("3. Back");
            _view.ShowMessages("4. Logout");

            var choice = _view.ReadInput("\nEnter your choice: ");

            switch (choice)
            {
                case "1":
                    await ShowNotifications();
                    break;
                case "2":
                    await ConfigureNotifications();
                    break;
            }
        }

        public async Task ShowNotifications()
        {
            _view.ShowMessages("\nYour Notifications:");
            _view.ShowMessages("No new notifications.");
            await Task.CompletedTask;
        }

        public async Task ConfigureNotifications()
        {
            _view.ShowMessages("\nC O N F I G U R E - N O T I F I C A T I O N S");
            _view.ShowMessages("1. Business - Enabled");
            _view.ShowMessages("2. Entertainment - Enabled");
            _view.ShowMessages("3. Sports - Disabled");
            _view.ShowMessages("4. Technology - Disabled");
            _view.ShowMessages("5. Keywords - Enabled");
            _view.ShowMessages("6. Back");
            _view.ShowMessages("7. Logout");

            var choice = _view.ReadInput("\nEnter your choice: ");

            if (choice == "5")
            {
                var keywords = _view.ReadInput("\nEnter keywords (comma-separated): ");
                _view.ShowMessages("\nKeywords updated successfully!");
            }

            await Task.CompletedTask;
        }

        public async Task SaveArticle(string articleId)
        {
            _view.ShowMessages($"\nArticle {articleId} saved successfully!");
            await Task.CompletedTask;
        }

        public async Task DeleteSavedArticle(string articleId)
        {
            _view.ShowMessages($"\nArticle {articleId} deleted successfully!");
            await Task.CompletedTask;
        }

        private void DisplayArticle(Article article)
        {
            _view.ShowMessages($"\nArticle Id: {article.Id}");
            _view.ShowMessages(article.Title);
            _view.ShowMessages(article.Description);
            _view.ShowMessages($"source: {article.Source}");
            _view.ShowMessages($"URL: {article.Url}");
            _view.ShowMessages($"Category: {article.Category}");
        }

        private async Task ShowArticleOptions(string articleId)
        {
            _view.ShowMessages("\n1. Back");
            _view.ShowMessages("2. Logout");
            _view.ShowMessages("3. Save Article");

            var choice = _view.ReadInput("\nEnter your choice: ");

            switch (choice)
            {
                case "3":
                    await SaveArticle(articleId);
                    break;
            }
        }
    }
} 