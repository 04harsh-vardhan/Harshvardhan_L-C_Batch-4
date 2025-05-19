using Moq;
using OnShopApi_s.Models;
using OnShopApi_s.Models.Dto;
using OnShopApi_s.Repositories;
using OnShopApi_s.Services;
using Xunit;

namespace OnShopApi_s.Tests.Services
{
    public class OrderServiceTests
    {
        private readonly Mock<IOnShopRepository> _mockRepo;
        private readonly OrderService _service;

        public OrderServiceTests()
        {
            _mockRepo = new Mock<IOnShopRepository>();
            _service = new OrderService(_mockRepo.Object);
        }

        [Fact]
        public async Task PlaceOrder_CallsRepositoryWithCorrectParameters()
        {
            var dto = new OrderPlaceDto
            {
                customerId = "cust01",
                productId = "prod01"
            };

            _mockRepo.Setup(repo => repo.CreateOrder(It.IsAny<Order>(), dto.productId)).Returns(Task.CompletedTask);

            await _service.PlaceOrder(dto);

            _mockRepo.Verify(repo => repo.CreateOrder(It.Is<Order>(o => o.customerId == dto.customerId), dto.productId), Times.Once);
        }

        [Fact]
        public async Task GetAllOrders_ReturnsUserOrdersWithOrderDetails()
        {
            string customerId = "cust01";
            var orders = new List<Order>
            {
                new Order(customerId) { orderId = "order1", customerId = customerId },
                new Order(customerId) { orderId = "order2", customerId = customerId }
            };

            var orderDetail1 = new OrderDetail("order1", "product1") { orderId = "order1" };
            var orderDetail2 = new OrderDetail("order2", "product2") { orderId = "order2" };

            _mockRepo.Setup(repo => repo.GetOrders(customerId)).ReturnsAsync(orders);
            _mockRepo.Setup(repo => repo.GetOrderDetail("order1")).ReturnsAsync(orderDetail1);
            _mockRepo.Setup(repo => repo.GetOrderDetail("order2")).ReturnsAsync(orderDetail2);

            var result = await _service.GetAllOrders(customerId);

            Assert.Equal(2, result.orderDetails.Count);
            Assert.Contains(result.orderDetails, d => d.orderId == "order1");
            Assert.Contains(result.orderDetails, d => d.orderId == "order2");

            _mockRepo.Verify(repo => repo.GetOrders(customerId), Times.Once);
            _mockRepo.Verify(repo => repo.GetOrderDetail("order1"), Times.Once);
            _mockRepo.Verify(repo => repo.GetOrderDetail("order2"), Times.Once);
        }
    }
}
