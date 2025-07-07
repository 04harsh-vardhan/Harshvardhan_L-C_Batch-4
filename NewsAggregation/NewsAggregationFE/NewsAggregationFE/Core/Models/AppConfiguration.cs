namespace NewsAggregationFE.Core.Models
{
    public class AppConfiguration
    {
        public ApiUrls ApiUrls { get; set; } = new();
        public ApplicationSettings ApplicationSettings { get; set; } = new();
    }

    public class ApiUrls
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string UserEndpoint { get; set; } = string.Empty;
        public string ServerEndpoint { get; set; } = string.Empty;
        public string CategoryEndpoint { get; set; } = string.Empty;
        public string ArticleEndpoint { get; set; } = string.Empty;
        public string NotificationEndpoint { get; set; } = string.Empty;
        public string ModeratedKeywordEndpoint { get; set; } = string.Empty;

        public string GetUserUrl() => BaseUrl + UserEndpoint;
        public string GetServerUrl() => BaseUrl + ServerEndpoint;
        public string GetCategoryUrl() => BaseUrl + CategoryEndpoint;
        public string GetArticleUrl() => BaseUrl + ArticleEndpoint;
        public string GetNotificationUrl() => BaseUrl + NotificationEndpoint;
        public string GetModeratedKeywordUrl() => BaseUrl + ModeratedKeywordEndpoint;
    }

    public class ApplicationSettings
    {
        public string AppName { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public int TimeoutSeconds { get; set; } = 30;
    }
}