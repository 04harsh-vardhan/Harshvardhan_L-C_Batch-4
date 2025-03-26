using OnShopApi_s.Models;

namespace OnShopApi_s.Services
{
    public interface IProductService
    {
        public Task<List<Product>> GetAllProducts();
    }
}
