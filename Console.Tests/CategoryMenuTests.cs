using Moq;
using Xunit;

namespace ConsoleApp1.Tests
{
    public class CategoryMenuTests
    {
        [Fact]
        public async Task Display_UserInputsValidIndex_CallsProductMenuDisplay()
        {
            var mockPrompt = new Prompts();
            mockPrompt._categories = new List<string> { "Electronics", "Clothing" };

            var mockHttp = new Mock<HttpClientRequests>();

            using var input = new StringReader("1\n");
            Console.SetIn(input);

            var categoryMenu = new TestableCategoryMenu(mockPrompt, mockHttp.Object);
            bool displayCalled = false;
            categoryMenu.ProductMenuFactory = (category, prompts, http) =>
            {
                var mock = new Mock<ProductMenu>(category, prompts, http);
                mock.Setup(p => p.Display()).Callback(() => displayCalled = true).Returns(Task.CompletedTask);
                return mock.Object;
            };

            await categoryMenu.Display();

            Assert.True(displayCalled);
        }

        [Fact]
        public async Task Display_UserInputsMinusOne_ExitsWithoutCallingProductMenu()
        {
            var mockPrompt = new Prompts();
            mockPrompt._categories = new List<string> { "Electronics", "Clothing" };

            var mockHttp = new Mock<HttpClientRequests>();

            using var input = new StringReader("-1\n");
            Console.SetIn(input);

            var categoryMenu = new TestableCategoryMenu(mockPrompt, mockHttp.Object);
            bool displayCalled = false;
            categoryMenu.ProductMenuFactory = (category, prompts, http) =>
            {
                var mock = new Mock<ProductMenu>(category, prompts, http);
                mock.Setup(p => p.Display()).Callback(() => displayCalled = true).Returns(Task.CompletedTask);
                return mock.Object;
            };

            await categoryMenu.Display();

            Assert.False(displayCalled);
        }
    }

    public class TestableCategoryMenu : CategoryMenu
    {
        public Func<string, Prompts, HttpClientRequests, ProductMenu> ProductMenuFactory { get; set; }

        public TestableCategoryMenu(Prompts prompts, HttpClientRequests http)
            : base(prompts, http)
        {
            ProductMenuFactory = (category, p, h) => new ProductMenu(category, p, h);
        }

        public new async Task Display()
        {
            Console.WriteLine("Categories");
            for (int index = 0; index < _prompt._categories.Count; index++)
            {
                Console.WriteLine($"{index} {_prompt._categories[index]}");
            }
            Console.WriteLine("Type the index to select the category");
            int userInput = Int32.Parse(Console.ReadLine() ?? "");
            if (userInput == -1)
            {
                return;
            }
            else
            {
                var productMenu = ProductMenuFactory(_prompt._categories[userInput], _prompt, _httpRequests);
                await productMenu.Display();
            }
        }
    }
}
