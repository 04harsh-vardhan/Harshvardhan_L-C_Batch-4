using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Test.Services
{
    public class ArticleServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IArticleRepository> _mockArticleRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ILogger<ArticleService>> _mockLogger;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IServiceProvider> _mockServiceProvider;
        private readonly ArticleService _service;

        public ArticleServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockArticleRepository = new Mock<IArticleRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<ArticleService>>();
            _mockEmailService = new Mock<IEmailService>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockServiceProvider = new Mock<IServiceProvider>();
            
            _service = new ArticleService(
                _mockConfiguration.Object,
                _mockArticleRepository.Object,
                _mockCategoryRepository.Object,
                _mockLogger.Object,
                _mockEmailService.Object,
                _mockUserRepository.Object,
                _mockServiceProvider.Object);
        }

        [Fact]
        public async Task GetSavedArticles_ReturnsFilteredArticles_BasedOnReportThreshold()
        {
            // Arrange
            var userId = 1;
            var articles = new List<Article>
            {
                new Article { Article_Id = 1, Article_Title = "Article 1", ReportCount = 2 },
                new Article { Article_Id = 2, Article_Title = "Article 2", ReportCount = 10 }
            };
            _mockArticleRepository.Setup(x => x.GetSavedArticles(userId)).ReturnsAsync(articles);
            _mockConfiguration.Setup(x => x.GetValue<int>("ArticleSettings:ReportThreshold", 5)).Returns(5);

            // Act
            var result = await _service.GetSavedArticles(userId);

            // Assert
            Assert.Single(result);
            Assert.Equal(1, result.First().Article_Id);
            _mockArticleRepository.Verify(x => x.GetSavedArticles(userId), Times.Once);
        }

        [Fact]
        public async Task LikeArticleAsync_ReturnsTrue_WhenArticleNotAlreadyLiked()
        {
            // Arrange
            var userId = 1;
            var articleId = 1;
            _mockArticleRepository.Setup(x => x.GetUserLikeForArticleAsync(userId, articleId)).ReturnsAsync((Like)null);
            _mockArticleRepository.Setup(x => x.AddOrUpdateLikeAsync(It.IsAny<Like>())).Returns(Task.CompletedTask);
            _mockArticleRepository.Setup(x => x.UpdateArticleLikeCountsAsync(articleId)).Returns(Task.CompletedTask);

            // Act
            var result = await _service.LikeArticleAsync(userId, articleId);

            // Assert
            Assert.True(result);
            _mockArticleRepository.Verify(x => x.AddOrUpdateLikeAsync(It.IsAny<Like>()), Times.Once);
            _mockArticleRepository.Verify(x => x.UpdateArticleLikeCountsAsync(articleId), Times.Once);
        }
    }
}