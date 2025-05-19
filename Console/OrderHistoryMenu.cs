namespace ConsoleApp1
{
    internal class OrderHistoryMenu
    {
        private readonly HttpClientRequests _httpRequests;
        private readonly Prompts _prompt;

        public OrderHistoryMenu(Prompts prompt, HttpClientRequests httpRequests)
        {
            _httpRequests = httpRequests;
            _prompt = prompt;
        }

        public async Task Display()
        {
            var allOrders = await _httpRequests.GetRequest(_prompt._GetCustomerOrdersURL + $"{Menu._customerId}");
            Console.WriteLine(allOrders);
            Console.ReadLine();
        }
    }
}
