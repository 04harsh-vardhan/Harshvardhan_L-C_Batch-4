using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregation.Controllers;
using NewsAggregation.Models;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Test.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly Mock<ILogger<CategoryController>> _mockLogger;
        private readonly CategoryController _controller;

        public CategoryControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _mockLogger = new Mock<ILogger<CategoryController>>();
            _controller = new CategoryController(_mockCategoryService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsOkResult_WithCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Category_Id = 1, Category_Name = "Technology", IsHidden = false },
                new Category { Category_Id = 2, Category_Name = "Sports", IsHidden = false }
            };
            _mockCategoryService.Setup(x => x.GetAllCategories()).ReturnsAsync(categories);

            // Act
            var result = await _controller.GetAllCategories();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedCategories = Assert.IsType<List<Category>>(okResult.Value);
            Assert.Equal(2, returnedCategories.Count);
            _mockCategoryService.Verify(x => x.GetAllCategories(), Times.Once);
        }
    }
}