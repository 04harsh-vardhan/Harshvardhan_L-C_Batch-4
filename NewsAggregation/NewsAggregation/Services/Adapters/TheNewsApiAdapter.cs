using NewsAggregation.Models;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;
using Newtonsoft.Json;

namespace NewsAggregation.Services.Adapters
{
    public class TheNewsApiAdapter : INewsApiAdapter
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;

        public string ApiName => "TheNewsAPI";

        public TheNewsApiAdapter(
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
                List<Article> articles = new();
                string apiUrl = _configuration["NewsApiUrls:TheNewsApi"] ?? "";
                string token = _configuration["NewsApiKeys:TheNewsApi"] ?? "";
                int page = 2;
                int pageCount = 1;
                while (pageCount < 20)
                {
                    apiUrl = BuildUrl(apiUrl, token, page);

                    TheNewsApi? theNewsApi = await GetApiDataAsync<TheNewsApi>(apiUrl);
                    if (theNewsApi?.Articles == null || !theNewsApi.Articles.Any())
                        break;

                    using var scope = _serviceProvider.CreateScope();
                    var categoryRepository = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
                    var articleRepository = scope.ServiceProvider.GetRequiredService<IArticleRepository>();

                    List<Category> allCategories = await categoryRepository.GetAllCategories();
                    articles.AddRange(await MapToArticleModelAsync(theNewsApi, allCategories, articleRepository));
                    page++;
                    pageCount++;
                }
                return articles;
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

        private async Task<List<Article>> MapToArticleModelAsync(TheNewsApi theNewsApi, List<Category> allCategories, IArticleRepository articleRepository)
        {
            List<Article> articles = new();

            foreach (Article1 theNewsApiArticle in theNewsApi.Articles)
            {
                Article article = new Article()
                {
                    Article_Title = theNewsApiArticle.Title,
                    Article_Description = theNewsApiArticle.Description,
                    Article_Source = theNewsApiArticle.Source,
                    Article_Url = theNewsApiArticle.Url
                };

                int articleId = await articleRepository.SaveArticleAndGetId(article);
                article.Article_Id = articleId;

                List<string> artCategories = theNewsApiArticle.Categories ?? new List<string>();
                foreach (string category in artCategories)
                {
                    int? categoryId = allCategories.FirstOrDefault(c =>
                        c.Category_Name.ToLower() == category.ToLower())?.Category_Id;

                    if (categoryId != null)
                    {
                        await articleRepository.SaveArticleWithCategory(
                            new ArticleCategory { ArticleId = articleId, CategoryId = (int)categoryId });
                    }
                }

                articles.Add(article);
            }

            return articles;
        }

        private static string BuildUrl(string url, string token, int page)
        {
            return $"{url}{token}&locale=us&limit={3}&page={page}";
        }
    }
}