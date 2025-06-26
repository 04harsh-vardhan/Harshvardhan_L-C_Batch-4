using Microsoft.AspNetCore.Mvc;
using NewsAggregation.Models.DTO;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _notificationService.CreateNotificationAsync(dto);
            return Ok(result);
        }

        [HttpGet("user-config/{userId}")]
        public async Task<IActionResult> GetUserNotificationConfig(int userId)
        {
            try
            {
                var result = await _notificationService.GetUserNotificationConfigAsync(userId);
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while fetching notification configuration" });
            }
        }
    }
}