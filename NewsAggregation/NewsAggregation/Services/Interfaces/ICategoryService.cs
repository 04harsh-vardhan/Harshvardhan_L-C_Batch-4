using NewsAggregation.Models;
using NewsAggregation.Models.DTO;

namespace NewsAggregation.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<bool> AddCategory(string category);
        public Task<List<Category>> GetAllCategories();
        public Task<bool> HideCategoryByNameAsync(string categoryName);
        public Task<List<CategoryStatusDto>> GetAllCategoriesStatusAsync();
        public Task<bool> UpdateCategoryStatusAsync(int categoryId, bool isEnabled);
    }
}
