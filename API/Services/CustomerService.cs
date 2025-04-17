using System.Text;
using OnShopApi_s.Models;
using OnShopApi_s.Repositories;

namespace OnShopApi_s.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IOnShopRepository _repository;
        public CustomerService(IOnShopRepository repository)
        {
            _repository = repository;
        }
        public async Task<string> LoginUser(string email, string password)
        {
            password = EncryptString(password);
            Customer customer = await _repository.LoginUser(email, password);
            if (customer == null)
            {
                throw new OnShopApi.HelperClasses.NoRecordException("No Customer present with these details");
            }
            return customer.customerId;
        }
        public async Task Signup(Customer customer)
        {
            customer.password = EncryptString(customer.password);
            await _repository.Signup(customer);
        }
        private string EncryptString(string key)
        {
            byte[] data = Encoding.UTF8.GetBytes(key);
            return Convert.ToBase64String(data);
        }
    }
}
