using Microsoft.EntityFrameworkCore;
using NewsAggregation.Models;
using NewsAggregation.Repository.Interfaces;

namespace NewsAggregation.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly NewsAggDBContext _dbContext;
        public CategoryRepository(NewsAggDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> GetCategoryIdByName(string categoryName)
        {
            return await _dbContext.Categories.Where(c => c.Category_Name.ToLower() == categoryName.ToLower()).Select(c => c.Category_Id)
                .FirstOrDefaultAsync();
        }
    }
}
