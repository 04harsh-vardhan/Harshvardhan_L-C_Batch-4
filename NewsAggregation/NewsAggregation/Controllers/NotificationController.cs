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

        [HttpPost("preferences")]
        public async Task<IActionResult> SetNotificationPreferences([FromBody] NotificationRequestDto request)
        {
            try
            {
                var result = await _notificationService.SetNotificationPreferences(request);
                return Ok(new { success = true, message = "Notification preferences set successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPut("preferences")]
        public async Task<IActionResult> UpdateNotificationPreferences([FromBody] NotificationRequestDto request)
        {
            try
            {
                var result = await _notificationService.UpdateNotificationPreferences(request);
                return Ok(new { success = true, message = "Notification preferences updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete("preferences/{userId}")]
        public async Task<IActionResult> DisableNotifications(int userId)
        {
            try
            {
                var result = await _notificationService.DisableNotifications(userId);
                return Ok(new { success = true, message = "Notifications disabled successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpPost("process/{categoryId}")]
        public async Task<IActionResult> ProcessNotifications(int categoryId)
        {
            try
            {
                await _notificationService.ProcessNewArticleNotifications(categoryId);
                return Ok(new { success = true, message = "Notifications processed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
