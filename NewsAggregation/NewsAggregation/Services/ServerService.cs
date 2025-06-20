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
        public async Task<bool> UpdateServerDetails(ServerDetailRequestBody serverDetail)
        {
            return await _serverRepository.UpdateServerApiKey(serverDetail.ServerId, serverDetail.ServerApi, serverDetail.ServerStatus);
        }
        public async Task<bool> AddServer(ExternalServerDto externalServerDto)
        {
            try
            {
                ExternalServer externalServer = new ExternalServer()
                {
                    Server_URL = externalServerDto.ServerURL,
                    Server_Name = externalServerDto.ServerName,
                    Server_Status = externalServerDto.ServerStatus,
                    Server_API_KEY = externalServerDto.ServerAPIKEY,
                    Last_accessed = externalServerDto.LastAccessed
                };
                await _serverRepository.AddServer(externalServer);
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }

        }
    }
}
