using NewsAggregation.Models;

namespace NewsAggregation.Repository.Interfaces
{
    public interface IServerRepository
    {
        public Task<List<ExternalServer>> GetAllServers();
        public Task<bool> updateServerApiKey(int serverID, string apiKey);
    }
}
