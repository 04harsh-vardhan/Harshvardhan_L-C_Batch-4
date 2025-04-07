using Microsoft.AspNetCore.Mvc;
using OnShopApi_s.Models;
using OnShopApi_s.Models.Dto;
using OnShopApi_s.Services;

namespace OnShopApi_s.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }




        /// <summary>
        /// {"email": "jane.smith@example.com","password": "StrongPass456" }
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] CustomerDto customer)
        {
            string customerId = await _customerService.LoginUser(customer.email, customer.password);
            if (!string.IsNullOrEmpty(customerId))
            {
                return Ok(customerId);
            }
            return BadRequest();
        }
        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] Customer customer)
        {
            await _customerService.Signup(customer);
            return Ok(true);
        }
    }
}
