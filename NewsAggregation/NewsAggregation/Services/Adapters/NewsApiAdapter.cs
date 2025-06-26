using NewsAggregation.Models;
using NewsAggregation.Models.NewsApi.Models;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;
using Newtonsoft.Json;

namespace NewsAggregation.Services.Adapters
{
    public class NewsApiAdapter : INewsApiAdapter
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public string ApiName => "NewsAPI";

        public NewsApiAdapter(
            IConfiguration configuration,
            IServiceProvider serviceProvider,
            IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Article>> FetchAndMapArticlesAsync()
        {
            try
            {
                string apiUrl = _configuration["NewsApiUrls:NewsApi"] ?? "";
                string token = _configuration["NewsApiKeys:NewsApi"] ?? "";
                apiUrl = BuildUrl(apiUrl, token);

                NewsApi? newsApi = await GetApiDataAsync<NewsApi>(apiUrl);
                if (newsApi?.Articles == null || !newsApi.Articles.Any())
                    return new List<Article>();

                using var scope = _serviceProvider.CreateScope();
                var categoryRepository = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
                var articleRepository = scope.ServiceProvider.GetRequiredService<IArticleRepository>();
                
                List<Category> allCategories = await categoryRepository.GetAllCategories();
                return await MapToArticleModelAsync(newsApi, allCategories, articleRepository);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to fetch articles from {ApiName}: {ex.Message}", ex);
            }
        }

        private async Task<T?> GetApiDataAsync<T>(string apiUrl)
        {
            using var httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode();
            string apiResponse = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(apiResponse);
        }

        private async Task<List<Article>> MapToArticleModelAsync(NewsApi newsApi, List<Category> allCategories, IArticleRepository articleRepository)
        {
            List<Article> articles = new();

            foreach (Article2 newsApiArticle in newsApi.Articles)
            {
                Article newArticle = new Article()
                {
                    Article_Description = newsApiArticle.Description,
                    Article_Title = newsApiArticle.Title,
                    Article_Source = newsApiArticle.Source?.Name,
                    Article_Url = newsApiArticle.Url
                };

                int articleId = await articleRepository.SaveArticleAndGetId(newArticle);
                newArticle.Article_Id = articleId;

                // Category matching based on description content
                foreach (Category category in allCategories)
                {
                    if (!string.IsNullOrEmpty(newArticle.Article_Description) && 
                        newArticle.Article_Description.Contains(category.Category_Name, StringComparison.OrdinalIgnoreCase))
                    {
                        await articleRepository.SaveArticleWithCategory(
                            new ArticleCategory { ArticleId = articleId, CategoryId = category.Category_Id });
                    }
                }

                articles.Add(newArticle);
            }

            return articles;
        }

        private string BuildUrl(string url, string token)
        {
            return url + token;
        }
    }
}