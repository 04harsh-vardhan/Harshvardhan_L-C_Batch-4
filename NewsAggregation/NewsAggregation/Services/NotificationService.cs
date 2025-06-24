using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IEmailService _emailService;
        private readonly IArticleRepository _articleRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly INotificationRepository _notificationRepository;

        public NotificationService(
            IEmailService emailService,
            IArticleRepository articleRepository,
            ICategoryRepository categoryRepository,
            INotificationRepository notificationRepository)
        {
            _emailService = emailService;
            _articleRepository = articleRepository;
            _categoryRepository = categoryRepository;
            _notificationRepository = notificationRepository;
        }


    }
}