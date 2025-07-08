using Microsoft.AspNetCore.Mvc;
using NewsAggregation.Models.DTO;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;
        
        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }
        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryReqBody categoryReqBody)
        {
            return Ok(await _categoryService.AddCategory(categoryReqBody.CategoryName));
        }
        [HttpGet("AllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            return Ok(await _categoryService.GetAllCategories());
        }

        /// <summary>
        /// Hides a category by name
        /// </summary>
        /// <param name="categoryName">The name of the category to hide</param>
        /// <returns>Success or failure result</returns>
        [HttpPost("hide/{categoryName}")]
        public async Task<IActionResult> HideCategory(string categoryName)
        {
            try
            {
                _logger.LogInformation("HideCategory request for CategoryName: {CategoryName}", categoryName);
                var result = await _categoryService.HideCategoryByNameAsync(categoryName);
                
                if (result)
                {
                    _logger.LogInformation("HideCategory completed successfully for CategoryName: {CategoryName}", categoryName);
                    return Ok(new { success = true, message = "Category hidden successfully" });
                }
                else
                {
                    _logger.LogWarning("HideCategory failed - Category not found: {CategoryName}", categoryName);
                    return NotFound(new { success = false, message = "Category not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HideCategory failed for CategoryName {CategoryName}: {Message}", categoryName, ex.Message);
                return StatusCode(500, new { success = false, message = "An error occurred while hiding the category" });
            }
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetCategoriesStatus()
        {
            try
            {
                _logger.LogInformation("GetCategoriesStatus request");
                var result = await _categoryService.GetAllCategoriesStatusAsync();
                _logger.LogInformation("GetCategoriesStatus completed - Found {Count} categories", result.Count);
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCategoriesStatus failed: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "An error occurred while fetching category status" });
            }
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateCategoryStatus([FromBody] UpdateCategoryStatusDto updateDto)
        {
            try
            {
                _logger.LogInformation("UpdateCategoryStatus request for CategoryId: {CategoryId}, IsEnabled: {IsEnabled}", updateDto.CategoryId, updateDto.IsEnabled);
                var result = await _categoryService.UpdateCategoryStatusAsync(updateDto.CategoryId, updateDto.IsEnabled);
                
                if (result)
                {
                    _logger.LogInformation("UpdateCategoryStatus completed successfully for CategoryId: {CategoryId}", updateDto.CategoryId);
                    return Ok(new { success = true, message = "Category status updated successfully" });
                }
                else
                {
                    _logger.LogWarning("UpdateCategoryStatus failed - Category not found: {CategoryId}", updateDto.CategoryId);
                    return NotFound(new { success = false, message = "Category not found" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateCategoryStatus failed for CategoryId {CategoryId}: {Message}", updateDto.CategoryId, ex.Message);
                return StatusCode(500, new { success = false, message = "An error occurred while updating category status" });
            }
        }
    }
}
