using NewsAggregationFE.Core.Models;

namespace NewsAggregationFE.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<List<ExternalServerList>> GetExternalServersList();
        public string ConcateServersDetails(List<ExternalServerList> externalServerDetails);
        public Task<List<ExternalServerDetail>> GetExternalServersDetails();
        public string ConcateList(List<ExternalServerDetail> externalServerDetails);
        public Task UpdateServer(string serverId, string apiKey);
        public Task AddNewCategory(string category);
    }
}
