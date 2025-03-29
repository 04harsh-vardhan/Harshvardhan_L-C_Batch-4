public class HttRequests
{
    public HttRequests()
    {

    }
    public async Task<string> GetBlogs(string bolgName, int startRange, int endRange)
    {
        string url = $"https://{bolgName}.tumblr.com/api/read/json?type=photo&num={endRange}&start={startRange}";
        HttpClient client = new HttpClient();
        HttpResponseMessage response = await client.GetAsync(url);
        string responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }
}
