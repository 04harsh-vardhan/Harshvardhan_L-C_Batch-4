using System.Net.Http.Json;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace NewsAggregationFE.Util
{
    public static class HttpRequest
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public static void SetAuthToken(string? token)
        {
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
            else
            {
                _httpClient.DefaultRequestHeaders.Authorization = null;
            }
        }

        public static async Task<T?> GetRequest<T>(string url, string? authToken = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(authToken))
                {
                    SetAuthToken(authToken);
                }

                HttpResponseMessage httpResponseMessage = await _httpClient.GetAsync(url);
                
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    string errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Request failed with status {httpResponseMessage.StatusCode}: {errorContent}");
                }

                string apiResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(apiResponse))
                {
                    return default(T);
                }

                T? response = JsonConvert.DeserializeObject<T>(apiResponse);
                return response;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during GET request: {ex.Message}", ex);
            }
        }

        public static async Task<TOut?> PostRequest<TIn, TOut>(TIn reqModel, string url, string? authToken = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(authToken))
                {
                    SetAuthToken(authToken);
                }

                HttpResponseMessage httpResponseMessage = await _httpClient.PostAsJsonAsync(url, reqModel);
                
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    string errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Request failed with status {httpResponseMessage.StatusCode}: {errorContent}");
                }

                string apiResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(apiResponse))
                {
                    return default(TOut);
                }

                TOut? response = JsonConvert.DeserializeObject<TOut>(apiResponse);
                return response;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during POST request: {ex.Message}", ex);
            }
        }

        public static async Task<TOut?> PutRequest<TIn, TOut>(TIn reqModel, string url, string? authToken = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(authToken))
                {
                    SetAuthToken(authToken);
                }

                HttpResponseMessage httpResponseMessage = await _httpClient.PutAsJsonAsync(url, reqModel);
                
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    string errorContent = await httpResponseMessage.Content.ReadAsStringAsync();
                    throw new HttpRequestException($"Request failed with status {httpResponseMessage.StatusCode}: {errorContent}");
                }

                string apiResponse = await httpResponseMessage.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(apiResponse))
                {
                    return default(TOut);
                }

                TOut? response = JsonConvert.DeserializeObject<TOut>(apiResponse);
                return response;
            }
            catch (HttpRequestException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred during PUT request: {ex.Message}", ex);
            }
        }
    }
}
