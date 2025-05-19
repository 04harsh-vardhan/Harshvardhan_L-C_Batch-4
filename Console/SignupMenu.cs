using ConsoleApp1.Models;

namespace ConsoleApp1
{
    internal class SignupMenu
    {
        private readonly Prompts _prompt;
        private readonly HttpClientRequests _httpRequests;
        public SignupMenu(Prompts prompts, HttpClientRequests httpRequests)
        {
            _prompt = prompts;
            _httpRequests = httpRequests;
        }
        public async Task Display()
        {
            SignupUserModel model = new SignupUserModel();
            model.customerId = Guid.NewGuid().ToString();
            Console.WriteLine("Enter your name");
            model.name = Console.ReadLine();
            Console.WriteLine("Enter your email");
            model.email = Console.ReadLine();
            Console.WriteLine("Enter your password");
            model.password = Console.ReadLine();
            Console.WriteLine("Enter your address");
            model.address = Console.ReadLine();
            Console.WriteLine("Enter your age");
            model.age = Int32.Parse(Console.ReadLine());
            await SignupUser(model);
        }
        private async Task SignupUser(SignupUserModel model)
        {
            string response = await _httpRequests.PostRequest(model, _prompt._SignupURL);
            Console.WriteLine("Signup Successfull");
        }
    }
}
