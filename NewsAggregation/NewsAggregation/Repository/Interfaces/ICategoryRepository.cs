using NewsAggregation.Models;

namespace NewsAggregation.Repository.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<int> GetCategoryIdByName(string categoryName);
        public Task<List<Category>> GetAllCategories();
        public Task<List<Category>> GetAllCategoriesIncludingHiddenAsync();
        public Task SaveCategory(Category category);
        public Task<Category?> GetCategoryByNameAsync(string categoryName);
        public Task<bool> HideCategoryAsync(int categoryId);
        public Task<bool> UpdateCategoryStatusAsync(int categoryId, bool isHidden);
    }
}
