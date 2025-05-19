using Microsoft.AspNetCore.Mvc;
using Moq;
using OnShopApi_s.Controllers;
using OnShopApi_s.Models;
using OnShopApi_s.Services;
using Xunit;

namespace OnShopApi.Test.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductController(_mockService.Object);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOk_WhenSuccessful()
        {
            // Arrange
            var mockProducts = new List<Product>
            {
                new Product { productId = "1", productName = "Product A", price = 100 },
                new Product { productId = "2", productName = "Product B", price = 200 }
            };

            _mockService.Setup(s => s.GetAllProducts()).ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<Product>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            _mockService.Setup(s => s.GetAllProducts())
                        .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Something went wrong", badRequest.Value);
        }
    }
}
