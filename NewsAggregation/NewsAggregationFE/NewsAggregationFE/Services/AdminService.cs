using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Services.Interfaces;
using NewsAggregationFE.Util;

namespace NewsAggregationFE.Services
{
    public class AdminService : IAdminService
    {
        private readonly string _getAllServersUrl;
        private readonly string _updateServerUrl;
        private readonly string _addCategoryUrl;

        public AdminService(AppConfiguration config)
        {
            var serverBaseUrl = config.ApiUrls.GetServerUrl();
            var categoryBaseUrl = config.ApiUrls.GetCategoryUrl();
            
            _getAllServersUrl = $"{serverBaseUrl}/GetAllServers";
            _updateServerUrl = $"{serverBaseUrl}/UpdateServer";
            _addCategoryUrl = $"{categoryBaseUrl}/AddCategory";
        }
        public async Task<List<ExternalServerList>> GetExternalServersList()
        {
            List<ExternalServer>? servers = await HttpRequest.GetRequest<List<ExternalServer>>(_getAllServersUrl);
            List<ExternalServerList> externalServerDetails = new List<ExternalServerList>();
            
            if (servers != null)
            {
                foreach (ExternalServer server in servers)
                {
                    externalServerDetails.Add(new ExternalServerList
                    {
                        ApiName = server.Server_Name,
                        Status = server.Server_Status ? "Active" : "Not Active",
                        LastAccessed = server.Last_accessed
                    });
                }
            }
            return externalServerDetails;
        }
        public string ConcateServersDetails(List<ExternalServerList> externalServerDetails)
        {
            string output = "";
            for (int i = 0; i < externalServerDetails.Count; i++)
            {
                var server = externalServerDetails[i];
                output += $"{i + 1}. {server.ApiName} - {server.Status} - last accessed: {server.LastAccessed:dd MMM yyyy}\n";
            }
            return output;
        }
        public async Task<List<ExternalServerDetail>> GetExternalServersDetails()
        {
            List<ExternalServer>? servers = await HttpRequest.GetRequest<List<ExternalServer>>(_getAllServersUrl);
            List<ExternalServerDetail> externalServerDetails = new List<ExternalServerDetail>();
            
            if (servers != null)
            {
                foreach (ExternalServer server in servers)
                {
                    externalServerDetails.Add(new ExternalServerDetail
                    {
                        ServerId = server.Server_ID,
                        ServerName = server.Server_Name,
                        ServerAPIKEY = server.Server_API_KEY
                    });
                }
            }
            return externalServerDetails;
        }
        public string ConcateList(List<ExternalServerDetail> externalServerDetails)
        {
            string output = string.Empty;
            for (int i = 0; i < externalServerDetails.Count; i++)
            {
                var server = externalServerDetails[i];
                output += $"{i + 1}. {server.ServerName} - {server.ServerAPIKEY}\n";
            }
            return output;
        }
        public async Task UpdateServer(string serverId, string apiKey)
        {
            try
            {
                int id = Int32.Parse(serverId);
                await HttpRequest.PutRequest<ServerDetailRequestBody, dynamic>(new ServerDetailRequestBody { ServerId = id, ServerApi = apiKey, ServerStatus = true }, _updateServerUrl);
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task AddNewCategory(string category)
        {
            try
            {
                await HttpRequest.PostRequest<CategoryReqBody, dynamic>(new CategoryReqBody { CategoryName = category }, _addCategoryUrl);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
