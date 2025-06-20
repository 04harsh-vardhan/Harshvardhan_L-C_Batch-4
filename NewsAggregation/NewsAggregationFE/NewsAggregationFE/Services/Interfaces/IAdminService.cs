using NewsAggregationFE.Core.Models;

namespace NewsAggregationFE.Services.Interfaces
{
    public interface IAdminService
    {
        public Task<List<ExternalServerDetails>> GetExternalServersDetails();
        public string ConcateServersDetails(List<ExternalServerDetails> externalServerDetails);
    }
}
