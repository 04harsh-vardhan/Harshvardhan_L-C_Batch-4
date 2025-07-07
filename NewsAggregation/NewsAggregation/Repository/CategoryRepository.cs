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
            return await _dbContext.Categories.Where(c => c.Category_Name.ToLower() == categoryName.ToLower() && !c.IsHidden).Select(c => c.Category_Id)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _dbContext.Categories.Where(c => !c.IsHidden).ToListAsync();
        }

        public async Task<List<Category>> GetAllCategoriesIncludingHiddenAsync()
        {
            return await _dbContext.Categories.ToListAsync();
        }
        public async Task SaveCategory(Category category)
        {
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Category?> GetCategoryByNameAsync(string categoryName)
        {
            return await _dbContext.Categories
                .FirstOrDefaultAsync(c => c.Category_Name.ToLower() == categoryName.ToLower());
        }

        public async Task<bool> HideCategoryAsync(int categoryId)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);
            if (category != null)
            {
                category.IsHidden = true;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateCategoryStatusAsync(int categoryId, bool isHidden)
        {
            var category = await _dbContext.Categories.FindAsync(categoryId);
            if (category != null)
            {
                category.IsHidden = isHidden;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
