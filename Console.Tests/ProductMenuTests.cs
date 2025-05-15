using Moq;
using Newtonsoft.Json;
using Xunit;

namespace ConsoleApp1.Tests
{
    public class ProductMenuTests
    {
        private readonly string _category = "Electronics";
        private readonly Prompts _prompt = new Prompts
        {
            _ProductsURL = "http://test/products",
            _PlaceOrderURL = "http://test/order"
        };

        private List<ProductModel> GetSampleProducts()
        {
            return new List<ProductModel>
            {
                new ProductModel { productId = "1", productName = "Phone", price = 999, category = "Electronics" },
                new ProductModel { productId = "2", productName = "T-Shirt", price = 19, category = "Clothing" },
                new ProductModel { productId = "3", productName = "Laptop", price = 1299, category = "Electronics" }
            };
        }

        [Fact]
        public async Task Display_ValidProductSelection_PlacesOrder()
        {
            // Arrange
            Menu._customerId = "cust-001";
            var mockHttp = new Mock<HttpClientRequests>();
            var allProducts = GetSampleProducts();
            var electronics = allProducts.FindAll(p => p.category == _category);
            var serializedProducts = JsonConvert.SerializeObject(allProducts);

            mockHttp.Setup(r => r.GetRequest(_prompt._ProductsURL))
                .ReturnsAsync(serializedProducts);

            mockHttp.Setup(r => r.PostRequest(It.IsAny<OrderPlaceModel>(), _prompt._PlaceOrderURL))
                .ReturnsAsync("Success");

            var productMenu = new ProductMenu(_category, _prompt, mockHttp.Object);

            // Simulate user selecting first electronics product (index 0)
            Console.SetIn(new StringReader("0"));

            // Act
            await productMenu.Display();

            // Assert
            mockHttp.Verify(r => r.PostRequest(
                It.Is<OrderPlaceModel>(o => o.productId == electronics[0].productId && o.customerId == "cust-001"),
                _prompt._PlaceOrderURL), Times.Once);
        }

        [Fact]
        public async Task Display_UserEntersMinusOne_ExitsGracefully()
        {
            // Arrange
            var mockHttp = new Mock<HttpClientRequests>();
            var allProducts = GetSampleProducts();
            var serializedProducts = JsonConvert.SerializeObject(allProducts);

            mockHttp.Setup(r => r.GetRequest(_prompt._ProductsURL))
                .ReturnsAsync(serializedProducts);

            var productMenu = new ProductMenu(_category, _prompt, mockHttp.Object);

            Console.SetIn(new StringReader("-1"));

            // Act
            var exception = await Record.ExceptionAsync(() => productMenu.Display());

            // Assert
            Assert.Null(exception); // Should return without error
            mockHttp.Verify(r => r.PostRequest(It.IsAny<OrderPlaceModel>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Display_InvalidIndex_ThrowsHandledException()
        {
            // Arrange
            var mockHttp = new Mock<HttpClientRequests>();
            var allProducts = GetSampleProducts();
            var serializedProducts = JsonConvert.SerializeObject(allProducts);

            mockHttp.Setup(r => r.GetRequest(_prompt._ProductsURL))
                .ReturnsAsync(serializedProducts);

            var productMenu = new ProductMenu(_category, _prompt, mockHttp.Object);

            // Provide an out-of-range index
            Console.SetIn(new StringReader("10"));

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => productMenu.Display());
            Assert.Null(exception); // Catches and logs inside method
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOnlyMatchingCategory()
        {
            // Arrange
            var mockHttp = new Mock<HttpClientRequests>();
            var allProducts = GetSampleProducts();
            var serializedProducts = JsonConvert.SerializeObject(allProducts);

            mockHttp.Setup(r => r.GetRequest(_prompt._ProductsURL))
                .ReturnsAsync(serializedProducts);

            var productMenu = new ProductMenu(_category, _prompt, mockHttp.Object);

            var method = typeof(ProductMenu)
                .GetMethod("GetAllProducts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            var result = await (Task<List<ProductModel>>)method.Invoke(productMenu, null);

            // Assert
            Assert.All(result, p => Assert.Equal("Electronics", p.category));
        }
    }
}
