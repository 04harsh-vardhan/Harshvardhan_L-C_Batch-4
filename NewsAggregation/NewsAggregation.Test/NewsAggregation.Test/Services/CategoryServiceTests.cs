using Moq;
using NewsAggregation.Models;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services;

namespace NewsAggregation.Test.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _service = new CategoryService(_mockCategoryRepository.Object);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsCategories_FromRepository()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Category_Id = 1, Category_Name = "Technology", IsHidden = false },
                new Category { Category_Id = 2, Category_Name = "Sports", IsHidden = false }
            };
            _mockCategoryRepository.Setup(x => x.GetAllCategories()).ReturnsAsync(categories);

            // Act
            var result = await _service.GetAllCategories();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("Technology", result.First().Category_Name);
            _mockCategoryRepository.Verify(x => x.GetAllCategories(), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryStatusAsync_ReturnsTrue_WhenRepositorySucceeds()
        {
            // Arrange
            var categoryId = 1;
            var isEnabled = false;
            _mockCategoryRepository.Setup(x => x.UpdateCategoryStatusAsync(categoryId, true)).ReturnsAsync(true);

            // Act
            var result = await _service.UpdateCategoryStatusAsync(categoryId, isEnabled);

            // Assert
            Assert.True(result);
            _mockCategoryRepository.Verify(x => x.UpdateCategoryStatusAsync(categoryId, true), Times.Once);
        }
    }
}