using NewsAggregation.Models;

namespace NewsAggregation.Utils
{
    public static class UserPreferenceFilter
    {
        public static List<Article> UserPrefFilter(this List<Article> articles, int userId, IServiceProvider serviceProvider)
        {
            try
            {
                NewsAggDBContext _db = serviceProvider.GetRequiredService<NewsAggDBContext>();
                int[] likedArticleIds = _db.Likes.Where(l => l.UserId == userId && l.Status == LikeStatus.Like).Select(l => l.ArticleId).ToArray();
                int[] enabledNotificationCategoriesIds = _db.Notifications.Where(n => n.UserId == userId && n.IsEnabled == true).Select(n => n.CategoryId).ToArray();
                List<Article> filteredArticles = articles.Where(a => likedArticleIds.Contains(a.Article_Id) ||
                enabledNotificationCategoriesIds.Intersect(a.ArticleCategories?.Select(ac => ac.CategoryId).ToArray() ?? new int[0]).ToArray().Length > 0).ToList();
                if (filteredArticles.Count < 3)
                {
                    Random random = new Random();
                    List<Article> randomArticlesPicks = new List<Article>();
                    foreach (Article article in articles)
                    {
                        int num = random.Next(1, 10);
                        if (num > 5)
                        {
                            randomArticlesPicks.Add(article);
                        }
                    }
                    filteredArticles.AddRange(randomArticlesPicks);
                }
                return filteredArticles;
            }
            catch (Exception)
            {
                return articles;
            }
        }
    }
}
