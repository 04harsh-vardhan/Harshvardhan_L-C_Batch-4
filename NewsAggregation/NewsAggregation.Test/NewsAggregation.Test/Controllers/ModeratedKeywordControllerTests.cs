using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregation.Controllers;
using NewsAggregation.Models.DTO;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Test.Controllers
{
    public class ModeratedKeywordControllerTests
    {
        private readonly Mock<IModeratedKeywordService> _mockService;
        private readonly Mock<ILogger<ModeratedKeywordController>> _mockLogger;
        private readonly ModeratedKeywordController _controller;

        public ModeratedKeywordControllerTests()
        {
            _mockService = new Mock<IModeratedKeywordService>();
            _mockLogger = new Mock<ILogger<ModeratedKeywordController>>();
            _controller = new ModeratedKeywordController(_mockService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllModeratedKeywords_ReturnsOkResult_WithKeywords()
        {
            // Arrange
            var keywords = new List<ModeratedKeywordDto>
            {
                new ModeratedKeywordDto { Id = 1, Keyword = "spam" },
                new ModeratedKeywordDto { Id = 2, Keyword = "inappropriate" }
            };
            _mockService.Setup(x => x.GetAllModeratedKeywordsAsync()).ReturnsAsync(keywords);

            // Act
            var result = await _controller.GetAllModeratedKeywords();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _mockService.Verify(x => x.GetAllModeratedKeywordsAsync(), Times.Once);
        }
    }
}