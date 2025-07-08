using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregation.Controllers;
using NewsAggregation.Models.DTO;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Test.Controllers
{
    public class NotificationControllerTests
    {
        private readonly Mock<INotificationService> _mockNotificationService;
        private readonly Mock<ILogger<NotificationController>> _mockLogger;
        private readonly NotificationController _controller;

        public NotificationControllerTests()
        {
            _mockNotificationService = new Mock<INotificationService>();
            _mockLogger = new Mock<ILogger<NotificationController>>();
            _controller = new NotificationController(_mockNotificationService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task GetUserNotificationConfig_ReturnsOkResult_WithConfig()
        {
            // Arrange
            var userId = 1;
            var config = new List<UserNotificationConfigDto>
            {
                new UserNotificationConfigDto { CategoryId = 1, CategoryName = "Technology", IsEnabled = true }
            };
            _mockNotificationService.Setup(x => x.GetUserNotificationConfigAsync(userId)).ReturnsAsync(config);

            // Act
            var result = await _controller.GetUserNotificationConfig(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _mockNotificationService.Verify(x => x.GetUserNotificationConfigAsync(userId), Times.Once);
        }
    }
}