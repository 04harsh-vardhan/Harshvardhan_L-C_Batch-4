using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services
{
    public class ModeratedKeywordService : IModeratedKeywordService
    {
        private readonly IModeratedKeywordRepository _moderatedKeywordRepository;
        private readonly ILogger<ModeratedKeywordService> _logger;

        public ModeratedKeywordService(
            IModeratedKeywordRepository moderatedKeywordRepository,
            ILogger<ModeratedKeywordService> logger)
        {
            _moderatedKeywordRepository = moderatedKeywordRepository;
            _logger = logger;
        }

        public async Task<List<ModeratedKeywordDto>> GetAllModeratedKeywordsAsync()
        {
            _logger.LogInformation("GetAllModeratedKeywords request");
            
            var keywords = await _moderatedKeywordRepository.GetAllModeratedKeywordsAsync();
            var result = keywords.Select(k => new ModeratedKeywordDto
            {
                Id = k.Id,
                Keyword = k.Keyword
            }).ToList();

            _logger.LogInformation("GetAllModeratedKeywords completed - Found {Count} keywords", result.Count);
            return result;
        }

        public async Task<ModeratedKeywordDto> AddModeratedKeywordAsync(string keyword)
        {
            _logger.LogInformation("AddModeratedKeyword request for keyword: {Keyword}", keyword);

            if (string.IsNullOrWhiteSpace(keyword))
            {
                _logger.LogWarning("AddModeratedKeyword called with empty keyword");
                throw new ArgumentException("Keyword cannot be empty");
            }

            var trimmedKeyword = keyword.Trim();
            
            var keywordExists = await _moderatedKeywordRepository.KeywordExistsAsync(trimmedKeyword);
            if (keywordExists)
            {
                _logger.LogWarning("AddModeratedKeyword failed - Keyword already exists: {Keyword}", trimmedKeyword);
                throw new InvalidOperationException("Keyword already exists");
            }

            var moderatedKeyword = await _moderatedKeywordRepository.AddModeratedKeywordAsync(trimmedKeyword);
            
            var result = new ModeratedKeywordDto
            {
                Id = moderatedKeyword.Id,
                Keyword = moderatedKeyword.Keyword
            };

            _logger.LogInformation("AddModeratedKeyword completed successfully for keyword: {Keyword}", trimmedKeyword);
            return result;
        }
    }
}