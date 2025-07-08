using NewsAggregation.Models;

namespace NewsAggregation.Services.Interfaces
{
    public interface INewsApiAdapter
    {
        string ApiName { get; }
        Task<List<Article>> FetchAndMapArticlesAsync();
    }
}