using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregation.Models;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services;

namespace NewsAggregation.Test.Services
{
    public class ModeratedKeywordServiceTests
    {
        private readonly Mock<IModeratedKeywordRepository> _mockRepository;
        private readonly Mock<ILogger<ModeratedKeywordService>> _mockLogger;
        private readonly ModeratedKeywordService _service;

        public ModeratedKeywordServiceTests()
        {
            _mockRepository = new Mock<IModeratedKeywordRepository>();
            _mockLogger = new Mock<ILogger<ModeratedKeywordService>>();
            _service = new ModeratedKeywordService(_mockRepository.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllModeratedKeywordsAsync_ReturnsKeywords_FromRepository()
        {
            // Arrange
            var keywords = new List<ModeratedKeywords>
            {
                new ModeratedKeywords { Id = 1, Keyword = "spam" },
                new ModeratedKeywords { Id = 2, Keyword = "inappropriate" }
            };
            _mockRepository.Setup(x => x.GetAllModeratedKeywordsAsync()).ReturnsAsync(keywords);

            // Act
            var result = await _service.GetAllModeratedKeywordsAsync();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("spam", result.First().Keyword);
            _mockRepository.Verify(x => x.GetAllModeratedKeywordsAsync(), Times.Once);
        }

        [Fact]
        public async Task AddModeratedKeywordAsync_ThrowsException_WhenKeywordExists()
        {
            // Arrange
            var keyword = "spam";
            _mockRepository.Setup(x => x.KeywordExistsAsync(keyword)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.AddModeratedKeywordAsync(keyword));
            _mockRepository.Verify(x => x.KeywordExistsAsync(keyword), Times.Once);
            _mockRepository.Verify(x => x.AddModeratedKeywordAsync(It.IsAny<string>()), Times.Never);
        }
    }
}