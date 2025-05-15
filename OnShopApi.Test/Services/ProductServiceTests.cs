using Moq;
using OnShopApi_s.Models;
using OnShopApi_s.Repositories;
using OnShopApi_s.Services;
using Xunit;

namespace OnShopApi_s.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IOnShopRepository> _mockRepo;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _mockRepo = new Mock<IOnShopRepository>();
            _service = new ProductService(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsListOfProducts()
        {
            var products = new List<Product>
            {
                new Product { productId = "p1", productName = "Product 1" },
                new Product { productId = "p2", productName = "Product 2" }
            };

            _mockRepo.Setup(repo => repo.GetAllProducts()).ReturnsAsync(products);

            var result = await _service.GetAllProducts();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.productId == "p1");
            Assert.Contains(result, p => p.productId == "p2");

            _mockRepo.Verify(repo => repo.GetAllProducts(), Times.Once);
        }
    }
}
