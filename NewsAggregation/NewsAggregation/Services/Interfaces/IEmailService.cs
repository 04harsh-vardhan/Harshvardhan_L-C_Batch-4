using NewsAggregation.Models;

namespace NewsAggregation.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendArticleNotificationAsync(string email, Article article);
        Task SendGroupedArticleNotificationsAsync(string email, string userName, List<Article> articles);
        Task SendArticleReportNotificationToAdminsAsync(Article article, List<User> adminUsers);
    }
}