
using comments;
using Main;
using Newtonsoft.Json;

public class Tumblr
{
    public static async Task Main()
    {
        Tumblr tumblr = new();
        await tumblr.Display();
    }
    public async Task Display()
    {
        string blogName = UserInput.GetBlogName();
        (int startRange, int endRange) = UserInput.GetRange();
        await GetBlogData(blogName, startRange, endRange);
    }
    private async Task GetBlogData(string blogName, int startRange, int endRange)
    {
        try
        {
            HttRequests httRequests = new();
            string blogData = await httRequests.GetBlogs(blogName, startRange, endRange);
            string filteredResponse = CleanResponse(blogData);
            // here we do not know the response model so we are using dynamic here
            dynamic responseModel = JsonConvert.DeserializeObject<dynamic>(filteredResponse);
            // Here we need only "title", "description","name","no of post"
            // so will only extract that data into a model
            TmblrResponseUI tmblrResponseUI = new TmblrResponseUI();
            FillTumblrResponseModel(tmblrResponseUI, responseModel);
            PrintData(tmblrResponseUI);
            Console.ReadKey();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Tmblr Api response is not in correct format");
            Console.WriteLine(ex.Message);
        }
    }
    // Need to clean the response as it is not a valid JSON response
    private static string CleanResponse(string response)
    {
        return response.Substring(22).Replace(";", "");
    }
    private static void FillTumblrResponseModel(TmblrResponseUI tmblrResponseUI, dynamic responseModel)
    {
        tmblrResponseUI.title = responseModel?.tumblelog?.title;
        tmblrResponseUI.description = responseModel?.tumblelog?.description;
        tmblrResponseUI.name = responseModel?.tumblelog?.name;
        tmblrResponseUI.numberOfPost = responseModel["post-total"];
        for (int postCount = 0; postCount < responseModel["posts"].Length; postCount++)
        {
            tmblrResponseUI.postUrls.Add(responseModel["posts"][postCount]["photo-url-1280"]);
        }
    }
    private static void PrintData(TmblrResponseUI tmblrResponseUI)
    {
        Console.WriteLine("title: " + tmblrResponseUI.title);
        Console.WriteLine("name: " + tmblrResponseUI.name);
        Console.WriteLine("description: " + tmblrResponseUI.description);
        Console.WriteLine("no of post: " + tmblrResponseUI.numberOfPost);
        int count = 0;
        foreach (var url in tmblrResponseUI.postUrls)
        {
            Console.WriteLine($"{count++}. {url}");
        }
    }
}