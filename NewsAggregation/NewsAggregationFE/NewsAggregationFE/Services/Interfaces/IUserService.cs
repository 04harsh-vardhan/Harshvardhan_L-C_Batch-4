using NewsAggregationFE.Core.Models;

namespace NewsAggregationFE.Services.Interfaces
{
    public interface IUserService
    {
        Task<ArticleResponse> GetNewsAsync(string? startDate = null, string? endDate = null, string? category = null);
        Task<List<Article>> SearchArticlesAsync(ArticleSearchRequest request);
        Task<List<Article>> GetSavedArticlesAsync(int userId);
        Task<bool> SaveArticleAsync(int userId, int articleId);
        Task<bool> LikeArticleAsync(int userId, int articleId);
        Task<bool> DislikeArticleAsync(int userId, int articleId);
        Task<bool> ReportArticleAsync(int articleId);
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
