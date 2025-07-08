using NewsAggregation.Models;
using NewsAggregation.Models.DTO;

namespace NewsAggregation.Services.Interfaces
{
    public interface IServerService
    {
        public Task<List<ExternalServer>> GetAllServers();
        public Task<bool> UpdateServerDetails(ServerDetailRequestBody serverDetail);
        public Task<bool> AddServer(ExternalServerDto externalServerDto);
    }
}
