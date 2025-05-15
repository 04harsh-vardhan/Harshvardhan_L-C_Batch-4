using Moq;
using Xunit;

namespace ConsoleApp1.Tests
{
    public class MenuTests
    {
        private Menu CreateMenuWithMocks(out Mock<LoginMenu> loginMock, out Mock<SignupMenu> signupMock, out Mock<CategoryMenu> categoryMock, out Mock<OrderHistoryMenu> orderHistoryMock)
        {
            var prompt = new Prompts();
            loginMock = new Mock<LoginMenu>(prompt, new HttpClientRequests());
            signupMock = new Mock<SignupMenu>(prompt, new HttpClientRequests());
            categoryMock = new Mock<CategoryMenu>(prompt, new HttpClientRequests());
            orderHistoryMock = new Mock<OrderHistoryMenu>(prompt, new HttpClientRequests());

            return new Menu(prompt, loginMock.Object, signupMock.Object, categoryMock.Object, orderHistoryMock.Object);
        }

        [Fact]
        public async Task Display_UserChoosesLogin_SuccessfulLogin()
        {
            var input = new StringReader("1\n"); // User selects login
            Console.SetIn(input);

            var menu = CreateMenuWithMocks(out var loginMock, out _, out _, out _);
            loginMock.Setup(x => x.Display()).ReturnsAsync("cust-001");

            var displayTask = menu.Display();

            await Task.Delay(300); // Let async calls process
            Assert.Equal("cust-001", Menu._customerId);
        }

        [Fact]
        public async Task Display_UserChoosesLogin_InvalidLogin()
        {
            var input = new StringReader("1\n"); // User selects login
            Console.SetIn(input);

            var menu = CreateMenuWithMocks(out var loginMock, out _, out _, out _);
            loginMock.Setup(x => x.Display()).ReturnsAsync("");

            var displayTask = menu.Display();

            await Task.Delay(300); // Let async calls process
            Assert.Equal("", Menu._customerId);
        }

        [Fact]
        public async Task Display_UserChoosesSignup_CallsSignupDisplay()
        {
            var input = new StringReader("2\n"); // Signup
            Console.SetIn(input);

            var menu = CreateMenuWithMocks(out _, out var signupMock, out _, out _);
            signupMock.Setup(x => x.Display()).Returns(Task.CompletedTask);

            var displayTask = menu.Display();

            await Task.Delay(300);
            signupMock.Verify(x => x.Display(), Times.Once);
        }

        [Fact]
        public async Task Display_UserChoosesCategory_ShowsOnlyIfAuthenticated()
        {
            Menu._customerId = "cust-001";
            var input = new StringReader("3\n"); // Show Categories
            Console.SetIn(input);

            var menu = CreateMenuWithMocks(out _, out _, out var categoryMock, out _);
            categoryMock.Setup(x => x.Display()).Returns(Task.CompletedTask);

            var displayTask = menu.Display();

            await Task.Delay(300);
            categoryMock.Verify(x => x.Display(), Times.Once);
        }

        [Fact]
        public async Task Display_UserChoosesOrderHistory_ShowsOnlyIfAuthenticated()
        {
            Menu._customerId = "cust-001";
            var input = new StringReader("4\n"); // Order history
            Console.SetIn(input);

            var menu = CreateMenuWithMocks(out _, out _, out _, out var historyMock);
            historyMock.Setup(x => x.Display()).Returns(Task.CompletedTask);

            var displayTask = menu.Display();

            await Task.Delay(300);
            historyMock.Verify(x => x.Display(), Times.Once);
        }

        [Fact]
        public async Task Display_UserChoosesLogout_CustomerIdCleared()
        {
            Menu._customerId = "cust-001";
            var input = new StringReader("5\n");
            Console.SetIn(input);

            var menu = CreateMenuWithMocks(out _, out _, out _, out _);

            await menu.Display();

            Assert.Equal("", Menu._customerId);
        }
    }
}
