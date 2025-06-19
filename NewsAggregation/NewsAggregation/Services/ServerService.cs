using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services
{
    public class ServerService : IServerService
    {
        private readonly IServerRepository _serverRepository;

        public ServerService(IServerRepository serverRepository)
        {
            _serverRepository = serverRepository;
        }
        public async Task<List<ExternalServer>> GetAllServers()
        {
            return await _serverRepository.GetAllServers();
        }
        public async Task<bool> UpdateServerApi(ServerDetailRequestBody serverDetail)
        {
            return await _serverRepository.updateServerApiKey(serverDetail.ServerId, serverDetail.ServerApi);
        }
    }
}
