using NewsAggregation.Models;

namespace NewsAggregation.Repository.Interfaces
{
    public interface IArticleRepository
    {
        public Task<List<Article>> GetFilteredNewsWithDate(DateTime startDate, DateTime endDate);
        public Task<bool> SaveArticles(List<Article> articles);
        public Task<List<Article>> GetSavedArticles(int userId);
        public Task<bool> SaveUserArticle(int userId, int articleId);
    }
}
