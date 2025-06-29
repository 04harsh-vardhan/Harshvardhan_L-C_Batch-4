using NewsAggregationFE.Controllers.Interfaces;
using NewsAggregationFE.Core.Interfaces;
using NewsAggregationFE.Core.Models;
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
            try
            {
                string choice = ShowWelcomeMessage();
                
                switch (choice)
                {
                    case "1":
                        await ShowHeadlinesMenu();
                        return true;
                        
                    case "2":
                        await ShowSavedArticles();
                        return true;
                        
                    case "3":
                        await ShowSearchMenu();
                        return true;
                        
                    case "4":
                        await ShowNotificationsMenu();
                        return true;
                        
                    case "5":
                        _consoleView.ShowMessages("Logging out...");
                        return false;
                        
                    default:
                        _consoleView.ShowMessages("Invalid option. Please try again.");
                        return true;
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"An error occurred: {ex.Message}");
                return true;
            }
        }

        private string ShowWelcomeMessage()
        {
            _consoleView.ShowMessages($"\nWelcome to the News Application, {_appState.Username}! Date: {DateTime.Now:dd-MMM-yyyy}");
            _consoleView.ShowMessages("");
            _consoleView.ShowMessages($"Time:{DateTime.Now:h:mmtt}");
            _consoleView.ShowMessages("");
            _consoleView.ShowMessages("Please choose the options below");
            _consoleView.ShowMessages("");
            _consoleView.ShowMessages("1. Headlines");
            _consoleView.ShowMessages("");
            _consoleView.ShowMessages("2. Saved Articles");
            _consoleView.ShowMessages("");
            _consoleView.ShowMessages("3. Search");
            _consoleView.ShowMessages("");
            _consoleView.ShowMessages("4. Notifications");
            _consoleView.ShowMessages("");
            _consoleView.ShowMessages("5. Logout");
            return _consoleView.ReadInput("");
        }

        private async Task ShowHeadlinesMenu()
        {
            try
            {
                string choice = _consoleView.ReadInput("\n--- Headlines Menu ---\n1. Today's headlines\n2. Date range headlines\n3. Headlines by category\n4. Back to main menu\nEnter your choice:");
                
                switch (choice)
                {
                    case "1":
                        await GetTodaysHeadlines();
                        break;
                        
                    case "2":
                        await GetHeadlinesByDateRange();
                        break;
                        
                    case "3":
                        await GetHeadlinesByCategory();
                        break;
                        
                    case "4":
                        return;
                        
                    default:
                        _consoleView.ShowMessages("Invalid option.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error loading headlines: {ex.Message}");
            }
        }

        private async Task GetTodaysHeadlines()
        {
            try
            {
                _consoleView.ShowMessages("\nFetching today's headlines...");
                var today = DateTime.Today.ToString("yyyy-MM-dd");
                var response = await _userService.GetNewsAsync(today, today);
                
                if (response.Success && response.Data.Any())
                {
                    DisplayArticles(response.Data);
                    await HandleArticleInteraction(response.Data);
                }
                else
                {
                    _consoleView.ShowMessages("No headlines found for today.");
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error fetching today's headlines: {ex.Message}");
            }
        }

        private async Task GetHeadlinesByDateRange()
        {
            try
            {
                string startDate = _consoleView.ReadInput("Enter start date (yyyy-MM-dd):");
                string endDate = _consoleView.ReadInput("Enter end date (yyyy-MM-dd):");
                
                _consoleView.ShowMessages($"\nFetching headlines from {startDate} to {endDate}...");
                var response = await _userService.GetNewsAsync(startDate, endDate);
                
                if (response.Success && response.Data.Any())
                {
                    DisplayArticles(response.Data);
                    await HandleArticleInteraction(response.Data);
                }
                else
                {
                    _consoleView.ShowMessages("No headlines found for the specified date range.");
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error fetching headlines by date range: {ex.Message}");
            }
        }

        private async Task GetHeadlinesByCategory()
        {
            try
            {
                var allCategories = await _userService.GetAllCategoriesAsync();
                var categories = allCategories.Where(c => !c.IsHidden).ToList();
                
                if (!categories.Any())
                {
                    _consoleView.ShowMessages("No categories available.");
                    return;
                }

                _consoleView.ShowMessages("\nAvailable categories:");
                for (int i = 0; i < categories.Count; i++)
                {
                    _consoleView.ShowMessages($"{i + 1}. {categories[i].Category_Name}");
                }

                string choice = _consoleView.ReadInput("Select category number:");
                if (int.TryParse(choice, out int categoryIndex) && categoryIndex > 0 && categoryIndex <= categories.Count)
                {
                    var selectedCategory = categories[categoryIndex - 1];
                    _consoleView.ShowMessages($"\nFetching headlines for category: {selectedCategory.Category_Name}...");
                    
                    var response = await _userService.GetNewsAsync(category: selectedCategory.Category_Name);
                    
                    if (response.Success && response.Data.Any())
                    {
                        DisplayArticles(response.Data);
                        await HandleArticleInteraction(response.Data);
                    }
                    else
                    {
                        _consoleView.ShowMessages($"No headlines found for category: {selectedCategory.Category_Name}");
                    }
                }
                else
                {
                    _consoleView.ShowMessages("Invalid category selection.");
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error fetching headlines by category: {ex.Message}");
            }
        }

        private async Task ShowSavedArticles()
        {
            try
            {
                _consoleView.ShowMessages("\nFetching your saved articles...");
                
                if (!_appState.UserId.HasValue)
                {
                    _consoleView.ShowMessages("Unable to retrieve user information.");
                    return;
                }

                int userId = _appState.UserId.Value;

                var savedArticles = await _userService.GetSavedArticlesAsync(userId);
                
                if (savedArticles.Any())
                {
                    DisplayArticles(savedArticles);
                    await HandleArticleInteraction(savedArticles);
                }
                else
                {
                    _consoleView.ShowMessages("You have no saved articles.");
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error fetching saved articles: {ex.Message}");
            }
        }

        private async Task ShowSearchMenu()
        {
            try
            {
                string keyword = _consoleView.ReadInput("Enter search keyword:");
                if (string.IsNullOrWhiteSpace(keyword))
                {
                    _consoleView.ShowMessages("Search keyword cannot be empty.");
                    return;
                }

                string useDate = _consoleView.ReadInput("Do you want to specify date range? (y/n):");
                DateTime? startDate = null;
                DateTime? endDate = null;

                if (useDate.ToLower() == "y")
                {
                    string startDateStr = _consoleView.ReadInput("Enter start date (yyyy-MM-dd) or press Enter to skip:");
                    string endDateStr = _consoleView.ReadInput("Enter end date (yyyy-MM-dd) or press Enter to skip:");

                    if (DateTime.TryParse(startDateStr, out DateTime start))
                        startDate = start;
                    if (DateTime.TryParse(endDateStr, out DateTime end))
                        endDate = end;
                }

                _consoleView.ShowMessages($"\nSearching for '{keyword}'...");
                
                var searchRequest = new ArticleSearchRequest
                {
                    Keyword = keyword,
                    StartDate = startDate,
                    EndDate = endDate
                };

                var articles = await _userService.SearchArticlesAsync(searchRequest);
                
                if (articles.Any())
                {
                    DisplayArticles(articles);
                    await HandleArticleInteraction(articles);
                }
                else
                {
                    _consoleView.ShowMessages("No articles found matching your search criteria.");
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error during search: {ex.Message}");
            }
        }

        private async Task ShowNotificationsMenu()
        {
            _consoleView.ShowMessages("Notifications feature is not implemented yet.");
            await Task.CompletedTask;
        }

        private void DisplayArticles(List<Article> articles)
        {
            _consoleView.ShowMessages($"\nFound {articles.Count} article(s):");
            _consoleView.ShowMessages(new string('-', 50));

            for (int i = 0; i < articles.Count; i++)
            {
                var article = articles[i];
                _consoleView.ShowMessages($"\n{i + 1}. {article.Article_Title}");
                if (!string.IsNullOrEmpty(article.Article_Description))
                    _consoleView.ShowMessages($"   Description: {article.Article_Description}");
                if (!string.IsNullOrEmpty(article.Article_Source))
                    _consoleView.ShowMessages($"   Source: {article.Article_Source}");
                _consoleView.ShowMessages($"   Date: {article.Created_At:yyyy-MM-dd HH:mm}");
                _consoleView.ShowMessages($"   Likes: {article.LikesCount}, Dislikes: {article.DislikesCount}");
            }
            _consoleView.ShowMessages(new string('-', 50));
        }

        private async Task HandleArticleInteraction(List<Article> articles)
        {
            try
            {
                string choice = _consoleView.ReadInput("\nWould you like to interact with an article? (y/n):");
                if (choice.ToLower() != "y") return;

                string articleChoice = _consoleView.ReadInput("Enter article number:");
                if (!int.TryParse(articleChoice, out int articleIndex) || articleIndex < 1 || articleIndex > articles.Count)
                {
                    _consoleView.ShowMessages("Invalid article number.");
                    return;
                }

                var selectedArticle = articles[articleIndex - 1];
                await ShowArticleActions(selectedArticle);
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error handling article interaction: {ex.Message}");
            }
        }

        private async Task ShowArticleActions(Article article)
        {
            try
            {
                if (!_appState.UserId.HasValue)
                {
                    _consoleView.ShowMessages("Unable to retrieve user information.");
                    return;
                }

                int userId = _appState.UserId.Value;

                string action = _consoleView.ReadInput($"\nSelected: {article.Article_Title}\n1. Save Article\n2. Like Article\n3. Dislike Article\n4. Report Article\n5. View URL\n6. Back\nChoose action:");

                switch (action)
                {
                    case "1":
                        bool saved = await _userService.SaveArticleAsync(userId, article.Article_Id);
                        _consoleView.ShowMessages(saved ? "Article saved successfully!" : "Failed to save article.");
                        break;

                    case "2":
                        bool liked = await _userService.LikeArticleAsync(userId, article.Article_Id);
                        _consoleView.ShowMessages(liked ? "Article liked successfully!" : "Failed to like article.");
                        break;

                    case "3":
                        bool disliked = await _userService.DislikeArticleAsync(userId, article.Article_Id);
                        _consoleView.ShowMessages(disliked ? "Article disliked successfully!" : "Failed to dislike article.");
                        break;

                    case "4":
                        bool reported = await _userService.ReportArticleAsync(article.Article_Id);
                        _consoleView.ShowMessages(reported ? "Article reported successfully!" : "Failed to report article.");
                        break;

                    case "5":
                        if (!string.IsNullOrEmpty(article.Article_Url))
                            _consoleView.ShowMessages($"Article URL: {article.Article_Url}");
                        else
                            _consoleView.ShowMessages("No URL available for this article.");
                        break;

                    case "6":
                        return;

                    default:
                        _consoleView.ShowMessages("Invalid action.");
                        break;
                }
            }
            catch (Exception ex)
            {
                _consoleView.ShowMessages($"Error performing article action: {ex.Message}");
            }
        }
    }
}