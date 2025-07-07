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
        private readonly string _getCategoriesStatusUrl;
        private readonly string _updateCategoryStatusUrl;
        private readonly string _getModeratedKeywordsUrl;
        private readonly string _addModeratedKeywordUrl;

        public AdminService(AppConfiguration config)
        {
            var serverBaseUrl = config.ApiUrls.GetServerUrl();
            var categoryBaseUrl = config.ApiUrls.GetCategoryUrl();
            var moderatedKeywordBaseUrl = config.ApiUrls.GetModeratedKeywordUrl();
            
            _getAllServersUrl = $"{serverBaseUrl}/GetAllServers";
            _updateServerUrl = $"{serverBaseUrl}/UpdateServer";
            _addCategoryUrl = $"{categoryBaseUrl}/AddCategory";
            _getCategoriesStatusUrl = $"{categoryBaseUrl}/status";
            _updateCategoryStatusUrl = $"{categoryBaseUrl}/status";
            _getModeratedKeywordsUrl = moderatedKeywordBaseUrl;
            _addModeratedKeywordUrl = moderatedKeywordBaseUrl;
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

        public async Task<List<CategoryStatusDto>> GetCategoriesStatusAsync()
        {
            try
            {
                var response = await HttpRequest.GetRequest<CategoryStatusResponse>(_getCategoriesStatusUrl);
                return response?.Data ?? new List<CategoryStatusDto>();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to get categories status: {e.Message}", e);
            }
        }

        public async Task<bool> UpdateCategoryStatusAsync(UpdateCategoryStatusDto updateDto)
        {
            try
            {
                var response = await HttpRequest.PutRequest<UpdateCategoryStatusDto, object>(updateDto, _updateCategoryStatusUrl);
                return response != null;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to update category status: {e.Message}", e);
            }
        }

        public async Task<List<ModeratedKeywordDto>> GetAllModeratedKeywordsAsync()
        {
            try
            {
                var response = await HttpRequest.GetRequest<ModeratedKeywordResponse>(_getModeratedKeywordsUrl);
                return response?.Data ?? new List<ModeratedKeywordDto>();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to get moderated keywords: {e.Message}", e);
            }
        }

        public async Task<bool> AddModeratedKeywordAsync(string keyword)
        {
            try
            {
                var addKeywordDto = new AddModeratedKeywordDto { Keyword = keyword };
                var response = await HttpRequest.PostRequest<AddModeratedKeywordDto, object>(addKeywordDto, _addModeratedKeywordUrl);
                return response != null;
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to add moderated keyword: {e.Message}", e);
            }
        }
    }
}
