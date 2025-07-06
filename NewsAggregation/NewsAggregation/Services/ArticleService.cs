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
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;
        
        public ArticleService(IConfiguration configuration, IArticleRepository articleRepository, ICategoryRepository categoryRepository, ILogger<ArticleService> logger, IEmailService emailService, IUserRepository userRepository)
        {
            _configuration = configuration;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _logger = logger;
            _emailService = emailService;
            _userRepository = userRepository;
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

                int reportThreshold = _configuration.GetValue<int>("ArticleSettings:ReportThreshold", 5);
                var filteredByReports = articles.Where(a => a.ReportCount < reportThreshold).ToList();
                _logger.LogInformation("Filtered out {Count} articles with report count >= {Threshold}", articles.Count - filteredByReports.Count, reportThreshold);

                if (string.IsNullOrEmpty(category))
                {
                    return filteredByReports;
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
                var filteredArticles = filteredByReports.Where(a => articleIdsSet.Contains(a.Article_Id)).ToList();
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
            
            int reportThreshold = _configuration.GetValue<int>("ArticleSettings:ReportThreshold", 5);
            var filteredResult = result.Where(a => a.ReportCount < reportThreshold).ToList();
            _logger.LogInformation("Found {Count} saved articles for UserId: {UserId}, filtered out {FilteredCount} articles with high report count", filteredResult.Count, userId, result.Count - filteredResult.Count);
            
            return filteredResult;
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
            
            int reportThreshold = _configuration.GetValue<int>("ArticleSettings:ReportThreshold", 5);
            var filteredResult = result.Where(a => a.ReportCount < reportThreshold).ToList();
            _logger.LogInformation("SearchArticlesAsync found {Count} articles, filtered out {FilteredCount} articles with high report count", filteredResult.Count, result.Count - filteredResult.Count);
            
            return filteredResult;
        }

        public async Task<bool> LikeArticleAsync(int userId, int articleId)
        {
            try
            {
                _logger.LogInformation("LikeArticle called for UserId: {UserId}, ArticleId: {ArticleId}", userId, articleId);

                var existingLike = await _articleRepository.GetUserLikeForArticleAsync(userId, articleId);

                if (existingLike != null && existingLike.Status == LikeStatus.Like)
                {
                    _logger.LogInformation("Article {ArticleId} already liked by User {UserId}", articleId, userId);
                    return false;
                }

                var like = new Like
                {
                    UserId = userId,
                    ArticleId = articleId,
                    Status = LikeStatus.Like
                };

                await _articleRepository.AddOrUpdateLikeAsync(like);
                await _articleRepository.UpdateArticleLikeCountsAsync(articleId);

                _logger.LogInformation("Article {ArticleId} liked successfully by User {UserId}", articleId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LikeArticle failed for UserId {UserId}, ArticleId {ArticleId}: {Message}", userId, articleId, ex.Message);
                return false;
            }
        }

        public async Task<bool> DislikeArticleAsync(int userId, int articleId)
        {
            try
            {
                _logger.LogInformation("DislikeArticle called for UserId: {UserId}, ArticleId: {ArticleId}", userId, articleId);

                var existingLike = await _articleRepository.GetUserLikeForArticleAsync(userId, articleId);

                if (existingLike != null && existingLike.Status == LikeStatus.DisLike)
                {
                    _logger.LogInformation("Article {ArticleId} already disliked by User {UserId}", articleId, userId);
                    return false;
                }

                var like = new Like
                {
                    UserId = userId,
                    ArticleId = articleId,
                    Status = LikeStatus.DisLike
                };

                await _articleRepository.AddOrUpdateLikeAsync(like);
                await _articleRepository.UpdateArticleLikeCountsAsync(articleId);

                _logger.LogInformation("Article {ArticleId} disliked successfully by User {UserId}", articleId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DislikeArticle failed for UserId {UserId}, ArticleId {ArticleId}: {Message}", userId, articleId, ex.Message);
                return false;
            }
        }

        public async Task<bool> ReportArticleAsync(int articleId)
        {
            try
            {
                _logger.LogInformation("ReportArticle called for ArticleId: {ArticleId}", articleId);

                var article = await _articleRepository.GetArticleByIdAsync(articleId);
                if (article == null)
                {
                    _logger.LogWarning("Article not found for ArticleId: {ArticleId}", articleId);
                    return false;
                }

                await _articleRepository.IncrementArticleReportCountAsync(articleId);

                var updatedArticle = await _articleRepository.GetArticleByIdAsync(articleId);
                if (updatedArticle == null)
                {
                    _logger.LogError("Failed to retrieve updated article after incrementing report count for ArticleId: {ArticleId}", articleId);
                    return false;
                }

                var adminUsers = await _userRepository.GetAllAdminUsersAsync();
                if (adminUsers.Any())
                {
                    await _emailService.SendArticleReportNotificationToAdminsAsync(updatedArticle, adminUsers);
                }
                else
                {
                    _logger.LogWarning("No admin users found to send report notification for ArticleId: {ArticleId}", articleId);
                }

                _logger.LogInformation("Article reported successfully - ArticleId: {ArticleId}, Total Reports: {ReportCount}", articleId, updatedArticle.ReportCount);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReportArticle failed for ArticleId {ArticleId}: {Message}", articleId, ex.Message);
                return false;
            }
        }
    }
}