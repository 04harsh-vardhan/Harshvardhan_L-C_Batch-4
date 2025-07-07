using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregation.Controllers;
using NewsAggregation.Models.DTO;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Test.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<ILogger<UserController>> _mockLogger;
        private readonly UserController _controller;

        public UserControllerTests()
        {
            _mockUserService = new Mock<IUserService>();
            _mockLogger = new Mock<ILogger<UserController>>();
            _controller = new UserController(_mockUserService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task LoginUser_ReturnsOkResult_WithValidCredentials()
        {
            // Arrange
            var loginRequest = new LoginUserRequestBody { Email = "test@example.com", Password = "password" };
            var loginResponse = new LoginResponseDto { Token = "jwt-token", Role = "User" };
            _mockUserService.Setup(x => x.LoginUser(loginRequest)).ReturnsAsync(loginResponse);

            // Act
            var result = await _controller.LoginUser(loginRequest);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
            _mockUserService.Verify(x => x.LoginUser(loginRequest), Times.Once);
        }
    }
}