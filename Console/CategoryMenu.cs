namespace ConsoleApp1
{
    internal class CategoryMenu
    {
        private readonly Prompts _prompt;
        private readonly HttpClientRequests _httpRequests;

        public CategoryMenu(Prompts prompts, HttpClientRequests httpClientRequests)
        {
            _prompt = prompts;
            _httpRequests = httpClientRequests;
        }
        public async Task Display()
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
                ProductMenu productMenu = new(_prompt._categories[userInput], _prompt, _httpRequests);
                await productMenu.Display();
            }
        }
    }
}
