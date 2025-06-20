using NewsAggregation.Models;

namespace NewsAggregation.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
        Task SendArticleNotificationAsync(string email, Article article);
    }
} 