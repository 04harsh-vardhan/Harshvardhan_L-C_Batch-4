using NewsAggregation.Models;
using NewsAggregation.Models.DTO;

namespace NewsAggregation.Services.Interfaces
{
    public interface IServerService
    {
        public Task<List<ExternalServer>> GetAllServers();
        public Task<bool> UpdateServerApi(ServerDetailRequestBody serverDetail);
    }
}
