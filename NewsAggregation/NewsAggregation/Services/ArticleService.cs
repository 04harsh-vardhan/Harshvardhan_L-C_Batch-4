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
        public ArticleService(IConfiguration configuration, IArticleRepository articleRepository, ICategoryRepository categoryRepository)
        {
            _configuration = configuration;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IList<Article>> GetNews(string startDate, string endDate, string? category)
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

            try
            {
                IList<Article> articles = await _articleRepository.GetFilteredNewsWithDate(startDate1, endDate1);

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
        public async Task<List<Article>> SearchArticlesAsync(ArticleSearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Keyword))
                throw new ArgumentException("Keyword is required");

            return await _articleRepository.SearchArticlesAsync(request);
        }







    }
}