using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregation.Controllers;
using NewsAggregation.Models;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Test.Controllers
{
    public class ArticleControllerTests
    {
        private readonly Mock<IArticleService> _mockArticleService;
        private readonly Mock<ILogger<ArticleController>> _mockLogger;
        private readonly ArticleController _controller;

        public ArticleControllerTests()
        {
            _mockArticleService = new Mock<IArticleService>();
            _mockLogger = new Mock<ILogger<ArticleController>>();
            _controller = new ArticleController(_mockArticleService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetNews_ReturnsOkResult_WithArticles()
        {
            // Arrange
            var articles = new List<Article>
            {
                new Article { Article_Id = 1, Article_Title = "Test Article", Article_Description = "Test Description" }
            };
            _mockArticleService.Setup(x => x.GetNews(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                              .ReturnsAsync(articles);

            // Act
            var result = await _controller.GetNews();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _mockArticleService.Verify(x => x.GetNews(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}