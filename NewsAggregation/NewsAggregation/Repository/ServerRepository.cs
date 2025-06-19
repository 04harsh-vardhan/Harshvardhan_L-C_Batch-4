using Microsoft.EntityFrameworkCore;
using NewsAggregation.Models;
using NewsAggregation.Repository.Interfaces;

namespace NewsAggregation.Repository
{
    public class ServerRepository : IServerRepository
    {
        private readonly NewsAggDBContext _dbContext;
        public ServerRepository(NewsAggDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ExternalServer>> GetAllServers()
        {
            return await _dbContext.ExternalServers.ToListAsync();
        }
        public async Task<bool> updateServerApiKey(int serverID, string apiKey)
        {
            ExternalServer? server = await _dbContext.ExternalServers.FirstOrDefaultAsync((server) => server.Server_ID == serverID);
            if (server != null)
            {
                server.Server_API_KEY = apiKey;
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
