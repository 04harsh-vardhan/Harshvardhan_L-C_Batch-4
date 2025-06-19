using Microsoft.EntityFrameworkCore;
using NewsAggregation.Models;
using NewsAggregation.Repository.Interfaces;

namespace NewsAggregation.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly NewsAggDBContext _dbContext;
        public ArticleRepository(NewsAggDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Article>> GetFilteredNewsWithDate(DateTime startDate, DateTime endDate)
        {
            return await _dbContext.Articles.Where(a => a.Created_At >= startDate && a.Created_At <= endDate).ToListAsync();
        }
        public async Task<bool> SaveArticles(List<Article> articles)
        {
            await _dbContext.AddRangeAsync(articles);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<Article>> GetSavedArticles(int userId)
        {
            List<SavedArticle> savedArticles = await _dbContext.SavedArticles.Where(sa => sa.UserId == userId).ToListAsync();
            return await _dbContext.Articles.Where(a => savedArticles.Any(sa => sa.ArticleId == a.Article_Id)).ToListAsync();
        }
        public async Task<bool> SaveUserArticle(int userId, int articleId)
        {
            try
            {
                await _dbContext.AddAsync(new SavedArticle { ArticleId = articleId, UserId = userId });
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
