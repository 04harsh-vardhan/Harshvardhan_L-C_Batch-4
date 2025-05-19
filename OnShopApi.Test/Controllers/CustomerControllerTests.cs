using Microsoft.AspNetCore.Mvc;
using Moq;
using OnShopApi.HelperClasses;
using OnShopApi_s.Controllers;
using OnShopApi_s.Models;
using OnShopApi_s.Models.Dto;
using OnShopApi_s.Services;
using Xunit;
namespace OnShopApi.Test.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _mockService;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _mockService = new Mock<ICustomerService>();
            _controller = new CustomerController(_mockService.Object);
        }

        [Fact]
        public async Task LoginUser_ReturnsOk_WhenLoginSuccessful()
        {
            var dto = new CustomerDto { email = "test@example.com", password = "pass" };
            _mockService.Setup(s => s.LoginUser(dto.email, dto.password)).ReturnsAsync("123");

            var result = await _controller.LoginUser(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("123", okResult.Value);
        }

        [Fact]
        public async Task LoginUser_ReturnsBadRequest_WhenNoRecordException()
        {
            var dto = new CustomerDto { email = "test@example.com", password = "pass" };
            _mockService.Setup(s => s.LoginUser(dto.email, dto.password))
                        .ThrowsAsync(new NoRecordException("No record found"));

            var result = await _controller.LoginUser(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("No record found", badRequest.Value);
        }

        [Fact]
        public async Task LoginUser_ReturnsBadRequest_WhenException()
        {
            var dto = new CustomerDto { email = "test@example.com", password = "pass" };
            _mockService.Setup(s => s.LoginUser(dto.email, dto.password))
                        .ThrowsAsync(new Exception("Something went wrong"));

            var result = await _controller.LoginUser(dto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Something went wrong", badRequest.Value);
        }

        [Fact]
        public async Task Signup_ReturnsOk_WhenSuccessful()
        {
            var customer = new Customer { /* populate minimal valid data */ };
            _mockService.Setup(s => s.Signup(customer)).Returns(Task.CompletedTask);

            var result = await _controller.Signup(customer);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task Signup_ReturnsBadRequest_WhenException()
        {
            var customer = new Customer { };
            _mockService.Setup(s => s.Signup(customer)).ThrowsAsync(new Exception("Failed to signup"));

            var result = await _controller.Signup(customer);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Failed to signup", badRequest.Value);
        }
    }
}
