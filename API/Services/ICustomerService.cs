using OnShopApi_s.Models;

namespace OnShopApi_s.Services
{
    public interface ICustomerService
    {
        public Task<string> LoginUser(string email, string password);
        public Task Signup(Customer customer);
    }
}
