using NewsAggregationFE.Core.Models;

namespace NewsAggregationFE.Core.Interfaces
{
    public interface IUserScreen
    {
        Task<bool> ShowMenu();
        Task ShowHeadlines(DateTime? startDate = null, DateTime? endDate = null);
        Task ShowSavedArticles();
        Task SearchArticles(string query, DateTime? startDate = null, DateTime? endDate = null);
        Task ShowNotifications();
        Task ConfigureNotifications();
        Task SaveArticle(string articleId);
        Task DeleteSavedArticle(string articleId);
    }
} 