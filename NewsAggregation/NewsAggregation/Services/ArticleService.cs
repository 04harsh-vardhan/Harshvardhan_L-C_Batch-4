using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IConfiguration _configuration;
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<ArticleService> _logger;
        
        public ArticleService(IConfiguration configuration, IArticleRepository articleRepository, ICategoryRepository categoryRepository, ILogger<ArticleService> logger)
        {
            _configuration = configuration;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task<IList<Article>> GetNews(string startDate, string endDate, string? category)
        {
            _logger.LogInformation("GetNews called with StartDate: {StartDate}, EndDate: {EndDate}, Category: {Category}", startDate, endDate, category);
            
            if (!DateTime.TryParse(startDate, out DateTime startDate1))
            {
                _logger.LogWarning("Invalid start date format: {StartDate}", startDate);
                throw new ArgumentException("Invalid start date format");
            }
            if (!DateTime.TryParse(endDate, out DateTime endDate1))
            {
                _logger.LogWarning("Invalid end date format: {EndDate}", endDate);
                throw new ArgumentException("Invalid end date format");
            }

            if (endDate1 < startDate1)
            {
                _logger.LogWarning("End date {EndDate} is earlier than start date {StartDate}", endDate, startDate);
                throw new ArgumentException("End date cannot be earlier than start date");
            }

            try
            {
                IList<Article> articles = await _articleRepository.GetFilteredNewsWithDate(startDate1, endDate1);
                _logger.LogInformation("Found {Count} articles between {StartDate} and {EndDate}", articles.Count, startDate, endDate);

                if (!articles.Any())
                {
                    return new List<Article>();
                }

                if (string.IsNullOrEmpty(category))
                {
                    return articles;
                }

                int categoryId = await _categoryRepository.GetCategoryIdByName(category);
                if (categoryId == 0)
                {
                    _logger.LogWarning("Category not found: {Category}", category);
                    throw new ArgumentException($"Category '{category}' not found");
                }

                var articleIdsForCategory = await _articleRepository.GetArticleIdsByCategoryId(categoryId);

                if (!articleIdsForCategory.Any())
                {
                    _logger.LogInformation("No articles found for category: {Category}", category);
                    return new List<Article>();
                }

                var articleIdsSet = articleIdsForCategory.ToHashSet();
                var filteredArticles = articles.Where(a => articleIdsSet.Contains(a.Article_Id)).ToList();
                _logger.LogInformation("Filtered to {Count} articles for category: {Category}", filteredArticles.Count, category);
                return filteredArticles;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching news articles");
                throw new Exception("An error occurred while fetching news articles", ex);
            }
        }


        public async Task<List<Article>> GetSavedArticles(int userId)
        {
            _logger.LogInformation("GetSavedArticles called for UserId: {UserId}", userId);
            var result = await _articleRepository.GetSavedArticles(userId);
            _logger.LogInformation("Found {Count} saved articles for UserId: {UserId}", result.Count, userId);
            return result;
        }

        public async Task<bool> SaveUserArticle(int userId, int articleId)
        {
            try
            {
                _logger.LogInformation("SaveUserArticle called for UserId: {UserId}, ArticleId: {ArticleId}", userId, articleId);
                var result = await _articleRepository.SaveUserArticle(userId, articleId);
                _logger.LogInformation("SaveUserArticle completed successfully for UserId: {UserId}", userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveUserArticle failed for UserId {UserId}, ArticleId {ArticleId}: {Message}", userId, articleId, ex.Message);
                return false;
            }
        }
        public async Task<List<Article>> SearchArticlesAsync(ArticleSearchRequest request)
        {
            _logger.LogInformation("SearchArticlesAsync called with Keyword: {Keyword}", request.Keyword);
            
            if (string.IsNullOrWhiteSpace(request.Keyword))
            {
                _logger.LogWarning("SearchArticlesAsync called with empty keyword");
                throw new ArgumentException("Keyword is required");
            }

            var result = await _articleRepository.SearchArticlesAsync(request);
            _logger.LogInformation("SearchArticlesAsync found {Count} articles", result.Count);
            return result;
        }

    }
}