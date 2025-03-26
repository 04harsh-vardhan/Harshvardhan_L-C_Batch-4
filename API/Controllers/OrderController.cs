using Microsoft.AspNetCore.Mvc;
using OnShopApi_s.Models.Dto;
using OnShopApi_s.Services;

namespace OnShopApi_s.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("placeOrder")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderPlaceDto orderPlaceDto)
        {
            await _orderService.PlaceOrder(orderPlaceDto);
            return Ok(true);
        }

        [HttpGet("getAllOrders/{customerId}")]
        public async Task<IActionResult> getOrders(string customerId)
        {
            UserOrders userOrders = await _orderService.GetAllOrders(customerId);
            return Ok(userOrders);
        }

    }
}
//{
//    "customerId": "1",
//  "productId": "P001"
//}