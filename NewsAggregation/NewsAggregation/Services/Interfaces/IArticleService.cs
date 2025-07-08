using NewsAggregation.Models;
using NewsAggregation.Models.DTO;

namespace NewsAggregation.Services.Interfaces
{
    public interface IArticleService
    {
        public Task<IList<Article>> GetNews(string startDate, string endDate, string? category);
        public Task<List<Article>> GetSavedArticles(int userId);
        public Task<bool> SaveUserArticle(int userId, int articleId);
        Task<List<Article>> SearchArticlesAsync(ArticleSearchRequest request);
        Task<bool> LikeArticleAsync(int userId, int articleId);
        Task<bool> DislikeArticleAsync(int userId, int articleId);
        Task<bool> ReportArticleAsync(int articleId);
    }
}
