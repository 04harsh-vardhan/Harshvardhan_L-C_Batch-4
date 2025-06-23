using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Services.Interfaces;
using NewsAggregationFE.Util;

namespace NewsAggregationFE.Services
{
    public class AdminService : IAdminService
    {
        private readonly string _getAllServersUrl = "https://localhost:7035/api/Server/GetAllServers";
        private readonly string _updateServerUrl = "https://localhost:7035/api/Server/UpdateServer";
        private readonly string _addCategoryUrl = "https://localhost:7035/api/Category/AddCategory";
        public async Task<List<ExternalServerList>> GetExternalServersList()
        {
            ExternalServerResponse? serverResponse = await HttpRequest.GetRequest<ExternalServerResponse>(_getAllServersUrl);
            List<ExternalServerList> externalServerDetails = new List<ExternalServerList>();
            foreach (Data data in serverResponse.Data)
            {
                externalServerDetails.Add(new ExternalServerList
                {
                    ApiName = data.ServerName
                    ,
                    Status = data.ServerStatus ? "Active" : "Not Active",
                    LastAccessed = data.LastAccessed
                });
            }
            return externalServerDetails;
        }
        public string ConcateServersDetails(List<ExternalServerList> externalServerDetails)
        {
            string output = "";
            foreach (var sd in externalServerDetails)
            {
                output += $"{sd.ApiName} - {sd.Status} - last accessed: {sd.LastAccessed}\n";
            }
            return output;
        }
        public async Task<List<ExternalServerDetail>> GetExternalServersDetails()
        {
            ExternalServerResponse? serverResponse = await HttpRequest.GetRequest<ExternalServerResponse>(_getAllServersUrl);
            List<ExternalServerDetail> externalServerDetails = new List<ExternalServerDetail>();
            foreach (Data data in serverResponse.Data)
            {
                externalServerDetails.Add(new ExternalServerDetail
                {
                    ServerId = data.ServerID,
                    ServerName = data.ServerName,
                    ServerAPIKEY = data.ServerAPIKEY
                }
                );
            }
            return externalServerDetails;
        }
        public string ConcateList(List<ExternalServerDetail> externalServerDetails)
        {
            string output = string.Empty;
            foreach (var sd in externalServerDetails)
            {
                output += $"{sd.ServerId}. {sd.ServerName} - {sd.ServerAPIKEY}\n";
            }
            return output;
        }
        public async Task UpdateServer(string serverId, string apiKey)
        {
            try
            {
                int id = Int32.Parse(serverId);
                await HttpRequest.Put<ServerDetailRequestBody, dynamic>(new ServerDetailRequestBody { ServerId = id, ServerApi = apiKey, ServerStatus = true }, _updateServerUrl);
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
                await HttpRequest.GetPost<CategoryReqBody, dynamic>(new CategoryReqBody { CategoryName = category }, _addCategoryUrl);
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
