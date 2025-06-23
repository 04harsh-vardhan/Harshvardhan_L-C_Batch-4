using System.Net.Http.Json;
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
                throw new Exception(ex.Message);
            }

        }
        public static async Task<TOut?> GetPost<TIn, TOut>(TIn reqModel, string url)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync(url, reqModel);
                httpResponseMessage.EnsureSuccessStatusCode();
                string apiResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                TOut? response = JsonConvert.DeserializeObject<TOut>(apiResponse);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public static async Task<TOut?> Put<TIn, TOut>(TIn reqModel, string url)
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync(url, reqModel);
                httpResponseMessage.EnsureSuccessStatusCode();
                string apiResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                TOut? response = JsonConvert.DeserializeObject<TOut>(apiResponse);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
