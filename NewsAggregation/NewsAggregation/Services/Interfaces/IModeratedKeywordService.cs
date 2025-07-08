using NewsAggregation.Models.DTO;

namespace NewsAggregation.Services.Interfaces
{
    public interface IModeratedKeywordService
    {
        Task<List<ModeratedKeywordDto>> GetAllModeratedKeywordsAsync();
        Task<ModeratedKeywordDto> AddModeratedKeywordAsync(string keyword);
    }
}