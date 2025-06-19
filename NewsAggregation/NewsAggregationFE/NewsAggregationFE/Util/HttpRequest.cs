using Newtonsoft.Json;

namespace NewsAggregationFE.Util
{
    public static class HttpRequest
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        public static async Task<T?> GetRequest<T>(string url)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url);
                httpResponseMessage.EnsureSuccessStatusCode();
                string apiResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                T? response = JsonConvert.DeserializeObject<T>(apiResponse);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception("Email or password is wrong");
            }

        }
    }
}
