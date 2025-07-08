using Microsoft.EntityFrameworkCore;
using NewsAggregation.Models;
using NewsAggregation.Repository.Interfaces;

namespace NewsAggregation.Repository
{
    public class ModeratedKeywordRepository : IModeratedKeywordRepository
    {
        private readonly NewsAggDBContext _dbContext;

        public ModeratedKeywordRepository(NewsAggDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<ModeratedKeywords>> GetAllModeratedKeywordsAsync()
        {
            return await _dbContext.ModeratedKeywords
                .OrderBy(mk => mk.Keyword)
                .ToListAsync();
        }

        public async Task<ModeratedKeywords> AddModeratedKeywordAsync(string keyword)
        {
            var moderatedKeyword = new ModeratedKeywords
            {
                Keyword = keyword.Trim()
            };

            await _dbContext.ModeratedKeywords.AddAsync(moderatedKeyword);
            await _dbContext.SaveChangesAsync();
            return moderatedKeyword;
        }

        public async Task<bool> KeywordExistsAsync(string keyword)
        {
            return await _dbContext.ModeratedKeywords
                .AnyAsync(mk => mk.Keyword.ToLower() == keyword.ToLower().Trim());
        }
    }
}