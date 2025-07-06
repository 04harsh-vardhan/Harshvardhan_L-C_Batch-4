using Microsoft.EntityFrameworkCore;
using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;

namespace NewsAggregation.Repository
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly NewsAggDBContext _dbContext;
        private readonly ILogger<ArticleRepository> _logger;
        
        public ArticleRepository(NewsAggDBContext dbContext, ILogger<ArticleRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        public async Task<List<Article>> GetFilteredNewsWithDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                _logger.LogInformation("GetFilteredNewsWithDate called for date range: {StartDate} to {EndDate}", startDate, endDate);
                var result = await _dbContext.Articles
                    .Where(a => a.Created_At >= startDate && a.Created_At <= endDate && !a.IsHidden)
                    .ToListAsync();
                _logger.LogInformation("Retrieved {Count} articles for date range", result.Count);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving filtered news for date range {StartDate} to {EndDate}", startDate, endDate);
                throw;
            }
        }
        public async Task<bool> SaveArticles(List<Article> articles)
        {
            try
            {
                _logger.LogInformation("SaveArticles called with {Count} articles", articles.Count);
                await _dbContext.AddRangeAsync(articles);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Successfully saved {Count} articles", articles.Count);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving {Count} articles", articles.Count);
                throw;
            }
        }
        public async Task<int> SaveArticleAndGetId(Article article)
        {
            try
            {
                _logger.LogDebug("SaveArticleAndGetId called for article: {Title}", article.Article_Title);
                await _dbContext.AddAsync(article);
                await _dbContext.SaveChangesAsync();
                _logger.LogDebug("Article saved with ID: {ArticleId}", article.Article_Id);
                return article.Article_Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving article: {Title}", article.Article_Title);
                throw;
            }
        }
        public async Task SaveArticleWithCategory(ArticleCategory articleCategory)
        {
            try
            {
                await _dbContext.AddAsync(articleCategory);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<Article>> GetSavedArticles(int userId)
        {
            var savedArticleIds = await _dbContext.SavedArticles
                .Where(sa => sa.UserId == userId)
                .Select(sa => sa.ArticleId)
                .ToListAsync();
            
            return await _dbContext.Articles
                .Where(a => savedArticleIds.Contains(a.Article_Id) && !a.IsHidden)
                .ToListAsync();
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

        public async Task<List<int>> GetArticleIdsByCategoryId(int categoryId)
        {
            return await _dbContext.ArticleCategories
                .Where(ac => ac.CategoryId == categoryId)
                .Select(ac => ac.ArticleId)
                .ToListAsync();
        }

        public async Task<List<Article>> GetNewArticlesSince(int categoryId, DateTime since)
        {
            var articleIds = await _dbContext.ArticleCategories
                .Where(ac => ac.CategoryId == categoryId)
                .Select(ac => ac.ArticleId)
                .ToListAsync();

            return await _dbContext.Articles
                .Where(a => articleIds.Contains(a.Article_Id) && a.Created_At > since && !a.IsHidden)
                .ToListAsync();
        }
        public async Task<List<Article>> SearchArticlesAsync(ArticleSearchRequest request)
        {
            var query = _dbContext.Articles
                .Where(a => !a.IsHidden && a.Article_Description != null &&
                            a.Article_Description.ToLower().Contains(request.Keyword.ToLower()));

            if (request.StartDate.HasValue)
                query = query.Where(a => a.Created_At >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(a => a.Created_At <= request.EndDate.Value);

            return await query
                .OrderByDescending(a => a.Created_At)
                .ToListAsync();
        }

        public async Task<Like?> GetUserLikeForArticleAsync(int userId, int articleId)
        {
            return await _dbContext.Likes
                .FirstOrDefaultAsync(l => l.UserId == userId && l.ArticleId == articleId);
        }

        public async Task AddOrUpdateLikeAsync(Like like)
        {
            var existingLike = await GetUserLikeForArticleAsync(like.UserId, like.ArticleId);
            
            if (existingLike != null)
            {
                existingLike.Status = like.Status;
                _dbContext.Likes.Update(existingLike);
            }
            else
            {
                await _dbContext.Likes.AddAsync(like);
            }
            
            await _dbContext.SaveChangesAsync();
        }

        public async Task RemoveLikeAsync(int userId, int articleId)
        {
            var like = await GetUserLikeForArticleAsync(userId, articleId);
            if (like != null)
            {
                _dbContext.Likes.Remove(like);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task UpdateArticleLikeCountsAsync(int articleId)
        {
            var article = await _dbContext.Articles.FindAsync(articleId);
            if (article != null)
            {
                var likeCount = await _dbContext.Likes
                    .CountAsync(l => l.ArticleId == articleId && l.Status == LikeStatus.Like);
                    
                var dislikeCount = await _dbContext.Likes
                    .CountAsync(l => l.ArticleId == articleId && l.Status == LikeStatus.DisLike);

                article.LikesCount = likeCount;
                article.DislikesCount = dislikeCount;
                
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Article?> GetArticleByIdAsync(int articleId)
        {
            return await _dbContext.Articles.FindAsync(articleId);
        }

        public async Task IncrementArticleReportCountAsync(int articleId)
        {
            var article = await _dbContext.Articles.FindAsync(articleId);
            if (article != null)
            {
                article.ReportCount++;
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
