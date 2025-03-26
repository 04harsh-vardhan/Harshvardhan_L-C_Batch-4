using Microsoft.EntityFrameworkCore;
using OnShopApi_s.Models;

namespace OnShopApi_s.Repositories
{
    public class OnShopRepository : IOnShopRepository
    {
        private readonly OnShopDbContext _db;
        public OnShopRepository(OnShopDbContext db)
        { _db = db; }

        public async Task<Customer> LoginUser(string email, string password)
        {
            Customer customer = await _db.Customers.FirstOrDefaultAsync(c => c.email == email && c.password == password);
            return customer;
        }

        public async Task Signup(Customer customer)
        {
            await _db.Customers.AddAsync(customer);
            await _db.SaveChangesAsync();
        }

        public async Task CreateOrder(Order order, string productId)
        {
            await _db.Orders.AddAsync(order);

            await _db.OrderDetails.AddAsync(new OrderDetail(order.orderId, productId));

            await _db.SaveChangesAsync();
        }
        public async Task<List<Order>> GetOrders(string customerId)
        {
            List<Order> orders = await _db.Orders.Where(o => o.customerId == customerId).ToListAsync();
            return orders;
        }

        public async Task<OrderDetail> GetOrderDetail(string orderId)
        {
            List<OrderDetail> orderDetails = await _db.OrderDetails.Where(od => od.orderId == orderId).ToListAsync();
            return orderDetails[0];
        }
        public async Task<List<Product>> GetAllProducts()
        {
            return await _db.Products.ToListAsync();
        }
    }
}
