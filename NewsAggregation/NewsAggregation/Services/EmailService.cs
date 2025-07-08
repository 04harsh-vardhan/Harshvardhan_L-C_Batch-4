using System.Net;
using System.Net.Mail;
using NewsAggregation.Models;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpUsername;
        private readonly string _smtpPassword;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _smtpHost = _configuration["EmailSettings:SmtpHost"];
            _smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            _smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            _smtpPassword = _configuration["EmailSettings:SmtpPassword"];
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                _logger.LogInformation("Sending email to {To} with subject: {Subject}", to, subject);
                
                var smtpClient = new SmtpClient(_smtpHost)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_smtpUsername, _smtpPassword),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_smtpUsername),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully to {To}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {To}: {Message}", to, ex.Message);
                throw;
            }
        }

        public async Task SendArticleNotificationAsync(string email, Article article)
        {
            var subject = $"New Article: {article.Article_Title}";
            var body = $@"
                <html>
                <body>
                    <h2>{article.Article_Title}</h2>
                    <p>{article.Article_Description}</p>
                    <p>Source: {article.Article_Source}</p>
                    <p><a href='{article.Article_Url}'>Read More</a></p>
                </body>
                </html>";

            await SendEmailAsync(email, subject, body);
        }

        public async Task SendGroupedArticleNotificationsAsync(string email, string userName, List<Article> articles)
        {
            try
            {
                _logger.LogInformation("Sending grouped article notifications to {Email} with {Count} articles", email, articles.Count);
                
                var subject = articles.Count == 1 
                    ? $"New Article Notification" 
                    : $"New Articles Notification - {articles.Count} articles";

                var articlesHtml = string.Join("", articles.Select(article => $@"
                    <div style='margin-bottom: 30px; padding: 20px; border: 1px solid #ddd; border-radius: 5px;'>
                        <h3 style='color: #333; margin-bottom: 10px;'>{article.Article_Title}</h3>
                        <p style='color: #666; margin-bottom: 10px;'>{article.Article_Description}</p>
                        <p style='margin-bottom: 10px;'><strong>Source:</strong> {article.Article_Source}</p>
                        <p><a href='{article.Article_Url}' style='color: #007bff; text-decoration: none;'>Read Full Article</a></p>
                    </div>"));

                var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #f8f9fa; padding: 20px; text-align: center; border-radius: 5px; margin-bottom: 20px; }}
                        .footer {{ margin-top: 30px; text-align: center; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1 style='color: #333; margin-bottom: 10px;'>News Aggregation Service</h1>
                            <p style='margin: 0; color: #666;'>Hello {userName}, you have {articles.Count} new article{(articles.Count > 1 ? "s" : "")} matching your interests!</p>
                        </div>
                        
                        {articlesHtml}
                        
                        <div class='footer'>
                            <p>This email was sent because you have active notifications for specific categories and keywords.</p>
                            <p>To manage your notification preferences, please log into your account.</p>
                        </div>
                    </div>
                </body>
                </html>";

                await SendEmailAsync(email, subject, body);
                _logger.LogInformation("Grouped article notification email sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send grouped article notifications to {Email}: {Message}", email, ex.Message);
                throw;
            }
        }

        public async Task SendArticleReportNotificationToAdminsAsync(Article article, List<User> adminUsers)
        {
            try
            {
                _logger.LogInformation("Sending article report notification to {Count} admin users for ArticleId: {ArticleId}", adminUsers.Count, article.Article_Id);

                var subject = "Article Reported";
                
                var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #dc3545; color: white; padding: 20px; text-align: center; border-radius: 5px; margin-bottom: 20px; }}
                        .article-details {{ background-color: #f8f9fa; padding: 20px; border-radius: 5px; margin-bottom: 20px; }}
                        .footer {{ margin-top: 30px; text-align: center; color: #666; font-size: 12px; }}
                        .report-info {{ background-color: #fff3cd; border: 1px solid #ffeaa7; padding: 15px; border-radius: 5px; margin-bottom: 20px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1 style='margin-bottom: 10px;'>Article Reported</h1>
                            <p style='margin: 0;'>An article has been reported and requires admin attention</p>
                        </div>
                        
                        <div class='report-info'>
                            <h3 style='color: #856404; margin-bottom: 10px;'>Report Information</h3>
                            <p><strong>Article ID:</strong> {article.Article_Id}</p>
                            <p><strong>Total Reports:</strong> {article.ReportCount}</p>
                            <p><strong>Report Time:</strong> {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
                        </div>
                        
                        <div class='article-details'>
                            <h3 style='color: #333; margin-bottom: 15px;'>Article Details</h3>
                            <p><strong>Title:</strong> {article.Article_Title}</p>
                            <p><strong>Description:</strong> {article.Article_Description}</p>
                            <p><strong>Source:</strong> {article.Article_Source}</p>
                            <p><strong>Published:</strong> {article.Created_At:yyyy-MM-dd HH:mm:ss} UTC</p>
                            <p><strong>Likes:</strong> {article.LikesCount} | <strong>Dislikes:</strong> {article.DislikesCount}</p>
                            <p><strong>Hidden:</strong> {(article.IsHidden ? "Yes" : "No")}</p>
                            <p><a href='{article.Article_Url}' style='color: #007bff; text-decoration: none;'>View Original Article</a></p>
                        </div>
                        
                        <div class='footer'>
                            <p>Please review this article and take appropriate action if necessary.</p>
                            <p>This is an automated notification from the News Aggregation System.</p>
                        </div>
                    </div>
                </body>
                </html>";

                foreach (var admin in adminUsers)
                {
                    await SendEmailAsync(admin.Email, subject, body);
                }

                _logger.LogInformation("Article report notification sent to {Count} admin users successfully", adminUsers.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send article report notification to admins: {Message}", ex.Message);
                throw;
            }
        }
    }
} 