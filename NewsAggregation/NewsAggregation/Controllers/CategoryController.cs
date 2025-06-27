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
    }
}
