using Microsoft.AspNetCore.Mvc;
using OnShopApi_s.Models;
using OnShopApi_s.Services;

namespace OnShopApi_s.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("products")]
        public async Task<IActionResult> GetAllProducts()
        {
            List<Product> products = await _productService.GetAllProducts();
            return Ok(products);
        }
    }
}
