using Moq;
using Xunit;

namespace ConsoleApp1.Tests
{
    public class LoginMenuTests
    {
        [Fact]
        public async Task Display_ValidCredentials_ReturnsCustomerId()
        {
            var mockPrompt = new Prompts { _LoginURL = "http://fake-login-url" };
            var mockHttp = new Mock<HttpClientRequests>();

            var input = new StringReader("john@example.com\n1234\n");
            Console.SetIn(input);

            mockHttp
                .Setup(x => x.PostRequest(It.IsAny<LoginUserModel>(), mockPrompt._LoginURL))
                .ReturnsAsync("cust123");

            var loginMenu = new LoginMenu(mockPrompt, mockHttp.Object);

            var result = await loginMenu.Display();

            Assert.Equal("cust123", result);

            mockHttp.Verify(x => x.PostRequest(
                It.Is<LoginUserModel>(m => m.email == "john@example.com" && m.password == "1234"),
                mockPrompt._LoginURL),
                Times.Once);
        }

        [Fact]
        public async Task Display_HttpRequestThrows_ReturnsEmptyString()
        {
            var mockPrompt = new Prompts { _LoginURL = "http://fake-login-url" };
            var mockHttp = new Mock<HttpClientRequests>();

            var input = new StringReader("error@example.com\nfail\n");
            Console.SetIn(input);

            mockHttp
                .Setup(x => x.PostRequest(It.IsAny<LoginUserModel>(), mockPrompt._LoginURL))
                .ThrowsAsync(new System.Exception("Network error"));

            var loginMenu = new LoginMenu(mockPrompt, mockHttp.Object);

            var result = await loginMenu.Display();

            Assert.Equal("", result);
        }

        [Fact]
        public async Task Display_EmptyInput_ReturnsEmptyString()
        {
            var mockPrompt = new Prompts { _LoginURL = "http://fake-login-url" };
            var mockHttp = new Mock<HttpClientRequests>();

            var input = new StringReader("\n\n");
            Console.SetIn(input);

            mockHttp
                .Setup(x => x.PostRequest(It.IsAny<LoginUserModel>(), mockPrompt._LoginURL))
                .ReturnsAsync("fake-response");

            var loginMenu = new LoginMenu(mockPrompt, mockHttp.Object);

            var result = await loginMenu.Display();

            Assert.Equal("fake-response", result);
        }
    }
}
