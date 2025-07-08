using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<bool> AddCategory(string category)
        {
            await _categoryRepository.SaveCategory(new Category { Category_Name = category });
            return true;
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _categoryRepository.GetAllCategories();
        }

        public async Task<bool> HideCategoryByNameAsync(string categoryName)
        {
            var category = await _categoryRepository.GetCategoryByNameAsync(categoryName);
            if (category == null)
            {
                return false;
            }
            
            return await _categoryRepository.HideCategoryAsync(category.Category_Id);
        }

        public async Task<List<CategoryStatusDto>> GetAllCategoriesStatusAsync()
        {
            var categories = await _categoryRepository.GetAllCategoriesIncludingHiddenAsync();
            return categories.Select(c => new CategoryStatusDto
            {
                CategoryId = c.Category_Id,
                CategoryName = c.Category_Name,
                IsEnabled = !c.IsHidden
            }).ToList();
        }

        public async Task<bool> UpdateCategoryStatusAsync(int categoryId, bool isEnabled)
        {
            return await _categoryRepository.UpdateCategoryStatusAsync(categoryId, !isEnabled);
        }
    }
}
