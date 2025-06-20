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
            if (!DateTime.TryParse(startDate, out DateTime startDate1))
            {
                throw new ArgumentException("Invalid start date format");
            }
            if (!DateTime.TryParse(endDate, out DateTime endDate1))
            {
                throw new ArgumentException("Invalid end date format");
            }

            if (endDate1 < startDate1)
            {
                throw new ArgumentException("End date cannot be earlier than start date");
            }

            var maxDateRange = TimeSpan.FromDays(30);
            if (endDate1 - startDate1 > maxDateRange)
            {
                throw new ArgumentException($"Date range cannot exceed {maxDateRange.Days} days");
            }

            try
            {
                IList<Article> articles = await _articleRepository.GetFilteredNewsWithDate(startDate1, endDate1);

                if (!articles.Any())
                {
                    return new List<Article>();
                }

                if (string.IsNullOrEmpty(category) || category.ToLower() == "general")
                {
                    return articles;
                }

                int categoryId = await _categoryRepository.GetCategoryIdByName(category);
                if (categoryId == 0)
                {
                    throw new ArgumentException($"Category '{category}' not found");
                }

                var articleIdsForCategory = await _articleRepository.GetArticleIdsByCategoryId(categoryId);

                if (!articleIdsForCategory.Any())
                {
                    return new List<Article>();
                }

                var articleIdsSet = articleIdsForCategory.ToHashSet();
                var filteredArticles = articles.Where(a => articleIdsSet.Contains(a.Article_Id)).ToList();
                return filteredArticles;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching news articles", ex);
            }
        }

        public async Task<bool> SyncNews()
        {
            await SyncTheNewsApi();
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
                List<Category> allCategories = await _categoryRepository.GetAllCategories();
                await MapTheNewsApiToArticleModel(theNewsApi, allCategories);
                return true;
            }
            catch (Exception ex)
            {
                throw;
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

        private async Task MapTheNewsApiToArticleModel(TheNewsApi theNewsApi, List<Category> allCategories)
        {
            foreach (Article1 theNewsApiArticle in theNewsApi.Articles)
            {
                Article article = new Article()
                {
                    Article_Title = theNewsApiArticle.Title,
                    Article_Description = theNewsApiArticle.Description,
                    Article_Source = theNewsApiArticle.Source,
                    Article_Url = theNewsApiArticle.Url
                };
                int articleId = await _articleRepository.SaveArticleAndGetId(article);
                List<string> artCategories = theNewsApiArticle.Categories;
                foreach (string category in artCategories)
                {
                    int? categoryId = allCategories.FirstOrDefault(c => c.Category_Name.ToLower() == category.ToLower())?.Category_Id;
                    if (categoryId != null)
                    {
                        await _articleRepository.SaveArticleWithCategory(new ArticleCategory { ArticleId = articleId, CategoryId = categoryId ?? 1 });
                    }
                }
            }
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
                    Article_Url = newsApiArticle.Url
                };
                articles.Add(newArticle);
            }
            return articles;
        }

        private static string BuildUrlTheNewsApi(string url, string token)
        {
            return $"{url}{token}&locale=us&limit={3}";
        }

        private string BuildUrlNewsApi(string url, string token)
        {
            return url + token;
        }
    }
}