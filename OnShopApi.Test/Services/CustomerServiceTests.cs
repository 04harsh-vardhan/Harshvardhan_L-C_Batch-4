using System.Text;
using Moq;
using OnShopApi.HelperClasses;
using OnShopApi_s.Models;
using OnShopApi_s.Repositories;
using OnShopApi_s.Services;
using Xunit;

namespace OnShopApi_s.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<IOnShopRepository> _mockRepo;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockRepo = new Mock<IOnShopRepository>();
            _service = new CustomerService(_mockRepo.Object);
        }

        [Fact]
        public async Task LoginUser_ValidCredentials_ReturnsCustomerId()
        {
            string email = "test@example.com";
            string password = "password123";
            string encryptedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
            var customer = new Customer { customerId = "cust123", email = email, password = encryptedPassword };

            _mockRepo.Setup(repo => repo.LoginUser(email, encryptedPassword)).ReturnsAsync(customer);

            string result = await _service.LoginUser(email, password);

            Assert.Equal("cust123", result);
            _mockRepo.Verify(repo => repo.LoginUser(email, encryptedPassword), Times.Once);
        }

        [Fact]
        public async Task LoginUser_InvalidCredentials_ThrowsNoRecordException()
        {
            string email = "test@example.com";
            string password = "wrongpassword";
            string encryptedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));

            _mockRepo.Setup(repo => repo.LoginUser(email, encryptedPassword)).ReturnsAsync((Customer)null);

            await Assert.ThrowsAsync<NoRecordException>(() => _service.LoginUser(email, password));
            _mockRepo.Verify(repo => repo.LoginUser(email, encryptedPassword), Times.Once);
        }

        [Fact]
        public async Task Signup_EncryptsPassword_AndCallsRepository()
        {
            string plainPassword = "myPassword";
            string encryptedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(plainPassword));
            var customer = new Customer { customerId = "cust001", email = "test@test.com", password = plainPassword };

            _mockRepo.Setup(repo => repo.Signup(It.IsAny<Customer>())).Returns(Task.CompletedTask);

            await _service.Signup(customer);

            _mockRepo.Verify(repo => repo.Signup(It.Is<Customer>(c => c.password == encryptedPassword)), Times.Once);
        }
    }
}
