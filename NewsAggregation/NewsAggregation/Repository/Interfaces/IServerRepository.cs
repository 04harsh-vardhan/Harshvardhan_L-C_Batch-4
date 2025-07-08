using NewsAggregation.Models;

namespace NewsAggregation.Repository.Interfaces
{
    public interface IServerRepository
    {
        public Task<List<ExternalServer>> GetAllServers();
        public Task<bool> UpdateServerApiKey(int serverID, string apiKey, bool serverStatus);
        public Task<bool> AddServer(ExternalServer externalServer);
    }
}
