using Microsoft.AspNetCore.Mvc;
using Moq;
using OnShopApi_s.Controllers;
using OnShopApi_s.Models.Dto;
using OnShopApi_s.Services;
using Xunit;

namespace OnShopApi.Test.Controllers
{
    public class OrderControllerTests
    {
        private readonly Mock<IOrderService> _mockService;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _mockService = new Mock<IOrderService>();
            _controller = new OrderController(_mockService.Object);
        }

        [Fact]
        public async Task PlaceOrder_ReturnsOk_WhenSuccessful()
        {
            var orderDto = new OrderPlaceDto { /* add minimal valid fields if required */ };
            _mockService.Setup(s => s.PlaceOrder(orderDto)).Returns(Task.CompletedTask);

            var result = await _controller.PlaceOrder(orderDto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.True((bool)okResult.Value);
        }

        [Fact]
        public async Task PlaceOrder_ReturnsBadRequest_WhenExceptionThrown()
        {
            var orderDto = new OrderPlaceDto { };
            _mockService.Setup(s => s.PlaceOrder(orderDto)).ThrowsAsync(new Exception("Order failed"));

            var result = await _controller.PlaceOrder(orderDto);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Order failed", badRequest.Value);
        }

        [Fact]
        public async Task GetOrders_ReturnsOk_WhenSuccessful()
        {
            string customerId = "123";
            var userOrders = new UserOrders { /* populate minimal mock orders if needed */ };
            _mockService.Setup(s => s.GetAllOrders(customerId)).ReturnsAsync(userOrders);

            var result = await _controller.getOrders(customerId);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(userOrders, okResult.Value);
        }

        [Fact]
        public async Task GetOrders_ReturnsBadRequest_WhenExceptionThrown()
        {
            string customerId = "123";
            _mockService.Setup(s => s.GetAllOrders(customerId)).ThrowsAsync(new Exception("Fetch failed"));

            var result = await _controller.getOrders(customerId);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Fetch failed", badRequest.Value);
        }
    }
}
