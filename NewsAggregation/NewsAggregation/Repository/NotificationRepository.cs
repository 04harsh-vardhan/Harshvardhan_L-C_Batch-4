using NewsAggregation.Models;
using NewsAggregation.Repository.Interfaces;

namespace NewsAggregation.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly NewsAggDBContext _dbContext;

        public NotificationRepository(NewsAggDBContext dbContext)
        {
            _dbContext = dbContext;
        }

    }
}