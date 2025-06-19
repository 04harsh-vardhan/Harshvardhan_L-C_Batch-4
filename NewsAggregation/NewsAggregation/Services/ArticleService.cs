using NewsAggregation.Models;
using NewsAggregation.Models.NewsApi.Models;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;
using Newtonsoft.Json;

namespace NewsAggregation.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IConfiguration _configuration;
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ArticleService(IConfiguration configuration, IArticleRepository articleRepository, ICategoryRepository categoryRepository)
        {
            _configuration = configuration;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IList<Article>> GetNews(string startDate, string endDate, string category)
        {
            DateTime.TryParse(startDate, out DateTime startDate1);
            DateTime.TryParse(endDate, out DateTime endDate1);
            IList<Article> articles = await _articleRepository.GetFilteredNewsWithDate(startDate1, endDate1);
            if (string.IsNullOrEmpty(category))
            {
                return articles;
            }
            int categoryId = await _categoryRepository.GetCategoryIdByName(category);
            return articles.Where(a => a.Category_Id == categoryId).ToList();
        }
        public async Task<bool> SyncNews()
        {
            //Implement multithreading for syncing news since both can be added to DB simultaneously
            Task<bool> syncTheNewsApi = Task.Run(() => SyncTheNewsApi());
            Task<bool> syncNewsApi = Task.Run(() => SyncNewsApi());
            Task.WaitAll(syncTheNewsApi, syncNewsApi);
            // Also check which endpoints are active and which are disabled
            // sync through TheNewsApi
            // sync through NewsApi
            return true;
        }
        public async Task<List<Article>> GetSavedArticles(int userId)
        {
            return await _articleRepository.GetSavedArticles(userId);
        }
        public async Task<bool> SaveUserArticle(int userId, int articleId)
        {
            try
            {
                return await _articleRepository.SaveUserArticle(userId, articleId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task<bool> SyncTheNewsApi()
        {
            try
            {
                string api_url = _configuration["NewsApiUrls:TheNewsApi"];
                string token = _configuration["NewsApiKeys:TheNewsApi"];
                api_url = BuildUrlTheNewsApi(api_url, token);
                TheNewsApi? theNewsApi = await GetApiData<TheNewsApi>(api_url);
                List<Article> articles = MapTheNewsApiToArticleModel(theNewsApi);
                return await _articleRepository.SaveArticles(articles);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task<bool> SyncNewsApi()
        {
            try
            {
                string api_url = _configuration["NewsApiUrls:NewsApi"];
                string token = _configuration["NewsApiKeys:NewsApi"];
                api_url = BuildUrlNewsApi(api_url, token);
                NewsApi? newsApi = await GetApiData<NewsApi>(api_url);
                List<Article> articles = MapNewsApiToArticleModel(newsApi);
                return await _articleRepository.SaveArticles(articles);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task<T?> GetApiData<T>(string api_url)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage httpResponse = await client.GetAsync(api_url);
            httpResponse.EnsureSuccessStatusCode();
            string apiResponse = await httpResponse.Content.ReadAsStringAsync();
            T? theNewsApi = JsonConvert.DeserializeObject<T>(apiResponse);
            return theNewsApi;
        }
        private List<Article> MapTheNewsApiToArticleModel(TheNewsApi theNewsApi)
        {
            List<Article> articles = new List<Article>();
            foreach (Article1 theNewsApiArticle in theNewsApi.Articles)
            {
                Article article = new Article()
                {
                    Article_uuid = theNewsApiArticle.Uuid,
                    Article_Title = theNewsApiArticle.Title,
                    Article_Description = theNewsApiArticle.Description,
                    Article_Source = theNewsApiArticle.Source,
                    Article_Url = theNewsApiArticle.Url
                };
                articles.Add(article);
            }
            return articles;
        }
        private List<Article> MapNewsApiToArticleModel(NewsApi newsApi)
        {
            List<Article> articles = new();
            foreach (Article2 newsApiArticle in newsApi.Articles)
            {
                Article newArticle = new Article()
                {
                    Article_Description = newsApiArticle.Description,
                    Article_Title = newsApiArticle.Title,
                    Article_Source = newsApiArticle.Source.Name,
                    Article_Url = newsApiArticle.Url,
                    Article_uuid = new Guid().ToString()
                };
                articles.Add(newArticle);
            }
            return articles;
        }
        private static string BuildUrlTheNewsApi(string url, string token)
        {
            return $"{url}/{token}&locale=us&limit={3}";
        }
        private string BuildUrlNewsApi(string url, string token)
        {
            return url + token;
        }

    }
}