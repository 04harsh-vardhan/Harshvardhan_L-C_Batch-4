namespace ConsoleApp1.Models
{
    internal class LoginUserModel
    {
        public string email;
        public string password;
        public LoginUserModel(string email, string password)
        {
            this.email = email;
            this.password = password;
        }
    }
}
