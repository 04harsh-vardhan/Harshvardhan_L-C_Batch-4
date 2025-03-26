using OnShopApi_s.Models;

namespace OnShopApi_s.Repositories
{
    public interface IOnShopRepository
    {
        public Task<Customer> LoginUser(string email, string password);
        public Task Signup(Customer customer);
        public Task CreateOrder(Order order, string orderDetail);
        public Task<List<Order>> GetOrders(string customerId);
        public Task<OrderDetail> GetOrderDetail(string orderId);
        public Task<List<Product>> GetAllProducts();
    }
}
