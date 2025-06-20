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
        public async Task<bool> UpdateServerApiKey(int serverID, string apiKey, bool serverStatus)
        {
            try
            {
                ExternalServer? server = await _dbContext.ExternalServers.FirstOrDefaultAsync((server) => server.Server_ID == serverID);
                if (server != null)
                {
                    server.Server_API_KEY = apiKey;
                    server.Server_Status = serverStatus;
                }
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<bool> AddServer(ExternalServer externalServer)
        {
            try
            {
                await _dbContext.ExternalServers.AddAsync(externalServer);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
