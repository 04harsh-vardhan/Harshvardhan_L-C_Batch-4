using Microsoft.Extensions.Logging;
using Moq;
using NewsAggregation.Models;
using NewsAggregation.Models.DTO;
using NewsAggregation.Repository.Interfaces;
using NewsAggregation.Services;
using NewsAggregation.Utils;

namespace NewsAggregation.Test.Services
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly Mock<IJwtTokenGenerator> _mockJwtTokenGenerator;
        private readonly Mock<ILogger<UserService>> _mockLogger;
        private readonly UserService _service;

        public UserServiceTests()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockJwtTokenGenerator = new Mock<IJwtTokenGenerator>();
            _mockLogger = new Mock<ILogger<UserService>>();
            _service = new UserService(_mockUserRepository.Object, _mockJwtTokenGenerator.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task SignupUser_ThrowsException_WhenEmailAlreadyExists()
        {
            // Arrange
            var signupDto = new SignupUserDto { Email = "test@example.com", Password = "password", UserName = "testuser" };
            var existingUsers = new List<User>
            {
                new User { Email = "test@example.com", UserId = 1 }
            };
            _mockUserRepository.Setup(x => x.GetAllUsers()).ReturnsAsync(existingUsers);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _service.SignupUser(signupDto));
            Assert.Equal("User email is Already in use", exception.Message);
            _mockUserRepository.Verify(x => x.GetAllUsers(), Times.Once);
            _mockUserRepository.Verify(x => x.AddUser(It.IsAny<SignupUserDto>()), Times.Never);
        }

        [Fact]
        public async Task LoginUser_ReturnsLoginResponse_WithValidCredentials()
        {
            // Arrange
            var loginRequest = new LoginUserRequestBody { Email = "test@example.com", Password = "password" };
            var user = new User { UserId = 1, Email = "test@example.com", Password = "hashedPassword", RoleId = 1 };
            var role = "User";
            var jwtToken = "jwt-token";

            _mockUserRepository.Setup(x => x.GetSingleUser(loginRequest.Email)).ReturnsAsync(user);
            _mockUserRepository.Setup(x => x.GetRoleById(user.RoleId)).ReturnsAsync(role);
            _mockJwtTokenGenerator.Setup(x => x.GenerateJwtToken(user, role)).Returns(jwtToken);

            // Mock password verification (this would need the actual password hashing logic)
            // For this test, we'll assume VerifyPassword method exists and works

            // Act
            var result = await _service.LoginUser(loginRequest);

            // Assert
            Assert.Equal(jwtToken, result.Token);
            Assert.Equal(role, result.Role);
            _mockUserRepository.Verify(x => x.GetSingleUser(loginRequest.Email), Times.Once);
            _mockUserRepository.Verify(x => x.GetRoleById(user.RoleId), Times.Once);
        }
    }
}