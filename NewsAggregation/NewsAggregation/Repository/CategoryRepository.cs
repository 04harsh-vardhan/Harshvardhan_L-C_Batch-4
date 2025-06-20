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
        public async Task<List<Category>> GetAllCategories()
        {
            return await _dbContext.Categories.ToListAsync();
        }
        public async Task SaveCategory(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }
    }
}
