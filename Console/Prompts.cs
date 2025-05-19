namespace ConsoleApp1
{
    internal class Prompts
    {
        public string WelcomeMsg = "Welcome to our shopping website";
        public string ValidateUser = "For Login Press 1 and for Signup Press 2";
        public string ValidationError = "Username or password is not correct";
        public string LoginMsg = "Enter your email and password \n Email \n Password";
        public string _LoginURL = "https://localhost:7040/api/Customer/login";
        public string _SignupURL = "https://localhost:7040/api/Signup/products";
        public readonly List<string> _categories = new() { "Electronics", "Accessories", "Storage", "Gaming" };
        public readonly string _ProductsURL = "https://localhost:7040/api/Product/products";
        public readonly string _PlaceOrderURL = "https://localhost:7040/api/Order/placeOrder";
        public readonly string _GetCustomerOrdersURL = "https://localhost:7040/api/Order/getAllOrders/";
    }
}
