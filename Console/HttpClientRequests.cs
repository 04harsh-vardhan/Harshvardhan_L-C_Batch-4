using System.Text;
using Newtonsoft.Json;

namespace ConsoleApp1
{
    internal class HttpClientRequests
    {
        public async Task<string> PostRequest<T>(T reqBody, string url)
        {
            HttpClient httpClient = new HttpClient();
            string jsonSerialize = JsonConvert.SerializeObject(reqBody);
            HttpContent content = new StringContent(jsonSerialize, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(url, content);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("unsuccess");
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        public async Task<string> GetRequest(string url)
        {
            HttpClient httpClient = new HttpClient();
            HttpResponseMessage responseMessage = await httpClient.GetAsync(url);
            string responseBody = await responseMessage.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
