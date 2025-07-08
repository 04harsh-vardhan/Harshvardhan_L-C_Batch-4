using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Services.Interfaces;
using NewsAggregationFE.State;
using NewsAggregationFE.Util;

namespace NewsAggregationFE.Services
{
    public class UserService : IUserService
    {
        private readonly AppState _appState;
        private readonly AppConfiguration _config;

        public UserService(AppState appState, AppConfiguration config)
        {
            _appState = appState;
            _config = config;
        }

        public async Task<ArticleResponse> GetNewsAsync(string? startDate = null, string? endDate = null, string? category = null)
        {
            try
            {
                var url = $"{_config.ApiUrls.GetArticleUrl()}/GetNews";
                var queryParams = new List<string>();
                
                if (!string.IsNullOrEmpty(startDate))
                    queryParams.Add($"startDate={startDate}");
                if (!string.IsNullOrEmpty(endDate))
                    queryParams.Add($"endDate={endDate}");
                if (!string.IsNullOrEmpty(category))
                    queryParams.Add($"category={category}");

                if (queryParams.Any())
                    url += "?" + string.Join("&", queryParams);

                var response = await HttpRequest.GetRequest<ArticleResponse>(url, _appState.JwtToken);
                return response ?? new ArticleResponse { Success = false, Message = "No response received" };
            }
            catch (Exception ex)
            {
                return new ArticleResponse { Success = false, Message = ex.Message };
            }
        }

        public async Task<List<Article>> SearchArticlesAsync(ArticleSearchRequest request)
        {
            try
            {
                var url = $"{_config.ApiUrls.GetArticleUrl()}/search";
                var queryParams = new List<string>();
                
                if (!string.IsNullOrEmpty(request.Keyword))
                    queryParams.Add($"keyword={Uri.EscapeDataString(request.Keyword)}");
                if (request.StartDate.HasValue)
                    queryParams.Add($"startDate={request.StartDate.Value:yyyy-MM-dd}");
                if (request.EndDate.HasValue)
                    queryParams.Add($"endDate={request.EndDate.Value:yyyy-MM-dd}");

                if (queryParams.Any())
                    url += "?" + string.Join("&", queryParams);

                var response = await HttpRequest.GetRequest<List<Article>>(url, _appState.JwtToken);
                return response ?? new List<Article>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Search failed: {ex.Message}", ex);
            }
        }

        public async Task<List<Article>> GetSavedArticlesAsync(int userId)
        {
            try
            {
                var url = $"{_config.ApiUrls.GetArticleUrl()}/GetSavedArticles?userId={userId}";
                var response = await HttpRequest.GetRequest<List<Article>>(url, _appState.JwtToken);
                return response ?? new List<Article>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get saved articles: {ex.Message}", ex);
            }
        }

        public async Task<bool> SaveArticleAsync(int userId, int articleId)
        {
            try
            {
                var url = $"{_config.ApiUrls.GetArticleUrl()}/SaveArticle";
                var request = new { UserId = userId, ArticleId = articleId };
                var response = await HttpRequest.PostRequest<object, object>(request, url, _appState.JwtToken);
                return response != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to save article: {ex.Message}", ex);
            }
        }

        public async Task<bool> LikeArticleAsync(int userId, int articleId)
        {
            try
            {
                var url = $"{_config.ApiUrls.GetArticleUrl()}/like";
                var request = new { UserId = userId, ArticleId = articleId };
                var response = await HttpRequest.PostRequest<object, object>(request, url, _appState.JwtToken);
                return response != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to like article: {ex.Message}", ex);
            }
        }

        public async Task<bool> DislikeArticleAsync(int userId, int articleId)
        {
            try
            {
                var url = $"{_config.ApiUrls.GetArticleUrl()}/dislike";
                var request = new { UserId = userId, ArticleId = articleId };
                var response = await HttpRequest.PostRequest<object, object>(request, url, _appState.JwtToken);
                return response != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to dislike article: {ex.Message}", ex);
            }
        }

        public async Task<bool> ReportArticleAsync(int articleId)
        {
            try
            {
                var url = $"{_config.ApiUrls.GetArticleUrl()}/report";
                var request = new { ArticleId = articleId };
                var response = await HttpRequest.PostRequest<object, object>(request, url, _appState.JwtToken);
                return response != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to report article: {ex.Message}", ex);
            }
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            try
            {
                var url = $"{_config.ApiUrls.GetCategoryUrl()}/AllCategories";
                var response = await HttpRequest.GetRequest<List<Category>>(url, _appState.JwtToken);
                return response ?? new List<Category>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get categories: {ex.Message}", ex);
            }
        }

        public async Task<List<Article>> GetPendingNotificationsAsync(int userId)
        {
            try
            {
                var url = $"{_config.ApiUrls.GetNotificationUrl()}/view/{userId}";
                var response = await HttpRequest.GetRequest<NotificationResponse>(url, _appState.JwtToken);
                return response?.Data ?? new List<Article>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get pending notifications: {ex.Message}", ex);
            }
        }

        public async Task<List<UserNotificationConfigDto>> GetUserNotificationConfigAsync(int userId)
        {
            try
            {
                var url = $"{_config.ApiUrls.GetNotificationUrl()}/user-config/{userId}";
                var response = await HttpRequest.GetRequest<NotificationConfigResponse>(url, _appState.JwtToken);
                return response?.Data ?? new List<UserNotificationConfigDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to get user notification config: {ex.Message}", ex);
            }
        }

        public async Task<bool> CreateNotificationAsync(CreateNotificationDto dto)
        {
            try
            {
                var url = $"{_config.ApiUrls.GetNotificationUrl()}/create";
                var response = await HttpRequest.PostRequest<CreateNotificationDto, object>(dto, url, _appState.JwtToken);
                return response != null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create notification: {ex.Message}", ex);
            }
        }
    }
}
