using NewsAggregation.Models;

namespace NewsAggregation.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<bool> AddCategory(string category);
        public Task<List<Category>> GetAllCategories();
        public Task<bool> HideCategoryByNameAsync(string categoryName);
    }
}
