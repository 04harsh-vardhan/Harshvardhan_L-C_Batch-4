using ConsoleApp1.Models;

namespace ConsoleApp1
{
    internal class LoginMenu
    {
        private readonly Prompts _prompt;
        private readonly HttpClientRequests _httpRequests;

        public LoginMenu(Prompts prompts, HttpClientRequests httpRequests)
        {
            _prompt = prompts;
            _httpRequests = httpRequests;
        }

        public async Task<string> Display()
        {
            Console.WriteLine("Type Your Email");
            string email = Console.ReadLine() ?? "";
            Console.WriteLine("Type Your Password");
            string password = Console.ReadLine() ?? "";
            return await LoginUser(email, password);
        }

        private async Task<string> LoginUser(string email, string password)
        {
            try
            {
                LoginUserModel loginModel = new(email, password);
                string response = await _httpRequests.PostRequest(loginModel, _prompt._LoginURL);

                return response;
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
