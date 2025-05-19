using OnShopApi_s.Models;
using OnShopApi_s.Repositories;

namespace OnShopApi_s.Services
{
    public class ProductService : IProductService
    {
        private readonly IOnShopRepository _repository;
        public ProductService(IOnShopRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<Product>> GetAllProducts()
        {
            return await _repository.GetAllProducts();
        }
    }
}
