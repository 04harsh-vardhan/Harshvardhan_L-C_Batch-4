namespace ConsoleApp1
{
    internal class Menu
    {
        public static string _customerId = "";
        private readonly Prompts _prompt;
        private readonly LoginMenu _loginMenu;
        private readonly SignupMenu _signupMenu;
        private readonly CategoryMenu _categoryMenu;
        private readonly OrderHistoryMenu _orderHistoryMenu;
        private readonly HttpClientRequests _httpClientRequests;
        public Menu(Prompts prompt)
        {
            _prompt = prompt;
            _httpClientRequests = new HttpClientRequests();
            _loginMenu = new LoginMenu(_prompt, _httpClientRequests);
            _signupMenu = new SignupMenu(_prompt, _httpClientRequests);
            _categoryMenu = new CategoryMenu(_prompt, _httpClientRequests);
            _orderHistoryMenu = new OrderHistoryMenu(_prompt, _httpClientRequests);
        }

        public async Task Display()
        {
            Console.WriteLine("Press 1 for Login");
            Console.WriteLine("Press 2 for Signup");
            Console.WriteLine("Press 3 To show All Categories");
            Console.WriteLine("Press 4 to show order history");
            Console.WriteLine("Press 5 to Logout");
            Console.WriteLine("Press -1 to go back to previous page");
            int userInput = Int32.Parse(Console.ReadLine());

            switch (userInput)
            {
                case 1:
                    await HandleLogin();
                    break;
                case 2:
                    await HandleSignup();
                    break;
                case 3:
                    await ShowCategories();
                    break;
                case 4:
                    await ShowOrderHistory();
                    break;
                case 5:
                    LogoutUser();
                    break;
                case -1:
                    await Display();
                    break;
                default:
                    Console.WriteLine("This is not valid UserInput");
                    break;
            }
        }
        private async Task HandleLogin()
        {
            _customerId = await _loginMenu.Display();
            if (string.IsNullOrEmpty(_customerId))
            {
                Console.WriteLine("Username or password is incorrect");
            }
            else
            {
                Console.WriteLine("Login Success");
            }
            await Task.Delay(2000);
            Console.Clear();
            await Display();
        }

        private async Task HandleSignup()
        {
            await _signupMenu.Display();
            await Task.Delay(2000);
            Console.Clear();
            await Display();
        }

        private async Task ShowCategories()
        {
            if (string.IsNullOrEmpty(_customerId))
            {
                Console.WriteLine("User is not authenticated");
            }
            await _categoryMenu.Display();
            await Task.Delay(2000);
            Console.Clear();
            await Display();
        }
        private async Task ShowOrderHistory()
        {
            if (string.IsNullOrEmpty(_customerId))
            {
                Console.WriteLine("User is not authenticated");
            }
            await _orderHistoryMenu.Display();
            await Task.Delay(2000);
            Console.Clear();
            await Display();
        }
        private async void LogoutUser()
        {
            _customerId = "";
            Console.WriteLine("Thankyou for shopping");
            await Task.Delay(2000);
        }

    }
}