using Microsoft.AspNetCore.Mvc;
using OnShopApi.HelperClasses;
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
            try
            {
                string customerId = await _customerService.LoginUser(customer.email, customer.password);
                return Ok(customerId);

            }
            catch (NoRecordException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] Customer customer)
        {
            try
            {
                await _customerService.Signup(customer);
                return Ok(true);
            }
            catch (Exception ex)
            { return BadRequest(ex.Message); }

        }
    }
}
