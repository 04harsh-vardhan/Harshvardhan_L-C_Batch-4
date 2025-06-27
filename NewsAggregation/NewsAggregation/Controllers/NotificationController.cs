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
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(INotificationService notificationService, ILogger<NotificationController> logger)
        {
            _notificationService = notificationService;
            _logger = logger;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                _logger.LogInformation("CreateNotification request for UserId: {UserId}, CategoryId: {CategoryId}", dto.UserId, dto.CategoryId);
                var result = await _notificationService.CreateNotificationAsync(dto);
                _logger.LogInformation("CreateNotification completed successfully for UserId: {UserId}", dto.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateNotification failed for UserId {UserId}: {Message}", dto.UserId, ex.Message);
                return StatusCode(500, "An error occurred while creating notification");
            }
        }

        [HttpGet("user-config/{userId}")]
        public async Task<IActionResult> GetUserNotificationConfig(int userId)
        {
            try
            {
                _logger.LogInformation("GetUserNotificationConfig request for UserId: {UserId}", userId);
                var result = await _notificationService.GetUserNotificationConfigAsync(userId);
                _logger.LogInformation("GetUserNotificationConfig completed - Found {Count} configurations for UserId: {UserId}", result.Count, userId);
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetUserNotificationConfig failed for UserId {UserId}: {Message}", userId, ex.Message);
                return StatusCode(500, new { success = false, message = "An error occurred while fetching notification configuration" });
            }
        }

        [HttpGet("view/{userId}")]
        public async Task<IActionResult> ViewNotifications(int userId)
        {
            try
            {
                _logger.LogInformation("ViewNotifications request for UserId: {UserId}", userId);
                var articles = await _notificationService.ViewNotificationsAsync(userId);
                _logger.LogInformation("ViewNotifications completed - Returned {Count} articles for UserId: {UserId}", articles.Count, userId);
                
                return Ok(new
                {
                    success = true,
                    data = articles,
                    message = $"Found {articles.Count} pending notifications"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ViewNotifications failed for UserId {UserId}: {Message}", userId, ex.Message);
                return StatusCode(500, new { success = false, message = "An error occurred while retrieving notifications" });
            }
        }
    }
}