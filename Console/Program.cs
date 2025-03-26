namespace ConsoleApp1
{
    class EComFrontEnd
    {
        private readonly Prompts _prompt;
        public static async Task Main(String[] args)
        {
            Prompts prompts = new();
            Menu menu = new(prompts);
            await menu.Display();
        }

    }
}