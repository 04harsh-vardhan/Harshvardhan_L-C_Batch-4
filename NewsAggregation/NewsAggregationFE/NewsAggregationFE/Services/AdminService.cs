using NewsAggregationFE.Core.Models;
using NewsAggregationFE.Services.Interfaces;
using NewsAggregationFE.Util;

namespace NewsAggregationFE.Services
{
    public class AdminService : IAdminService
    {
        private readonly string _getAllServersUrl = "https://localhost:7035/api/Server/GetAllServers";
        public async Task<List<ExternalServerDetails>> GetExternalServersDetails()
        {
            ExternalServerResponse? serverResponse = await HttpRequest.GetRequest<ExternalServerResponse>(_getAllServersUrl);
            List<ExternalServerDetails> externalServerDetails = new List<ExternalServerDetails>();
            foreach (Data data in serverResponse.Data)
            {
                externalServerDetails.Add(new ExternalServerDetails
                {
                    ApiName = data.ServerName
                    ,
                    Status = data.ServerStatus ? "Active" : "Not Active",
                    LastAccessed = data.LastAccessed
                });
            }
            return externalServerDetails;
        }
        public string ConcateServersDetails(List<ExternalServerDetails> externalServerDetails)
        {
            string output = "";
            foreach (var sd in externalServerDetails)
            {
                output += $"{sd.ApiName} {sd.Status} {sd.LastAccessed}\n";
            }
            return output;
        }
    }
}
