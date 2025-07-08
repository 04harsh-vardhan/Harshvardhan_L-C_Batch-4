using NewsAggregation.Models;

namespace NewsAggregation.Repository.Interfaces
{
    public interface IModeratedKeywordRepository
    {
        Task<List<ModeratedKeywords>> GetAllModeratedKeywordsAsync();
        Task<ModeratedKeywords> AddModeratedKeywordAsync(string keyword);
        Task<bool> KeywordExistsAsync(string keyword);
    }
}