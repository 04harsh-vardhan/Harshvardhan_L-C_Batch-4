using Microsoft.AspNetCore.Mvc;
using NewsAggregation.Models.DTO;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModeratedKeywordController : ControllerBase
    {
        private readonly IModeratedKeywordService _moderatedKeywordService;
        private readonly ILogger<ModeratedKeywordController> _logger;

        public ModeratedKeywordController(
            IModeratedKeywordService moderatedKeywordService,
            ILogger<ModeratedKeywordController> logger)
        {
            _moderatedKeywordService = moderatedKeywordService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllModeratedKeywords()
        {
            try
            {
                _logger.LogInformation("GetAllModeratedKeywords endpoint called");
                var keywords = await _moderatedKeywordService.GetAllModeratedKeywordsAsync();
                
                _logger.LogInformation("GetAllModeratedKeywords completed - Returned {Count} keywords", keywords.Count);
                return Ok(new
                {
                    success = true,
                    data = keywords
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllModeratedKeywords failed: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "An error occurred while fetching moderated keywords" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddModeratedKeyword([FromBody] AddModeratedKeywordDto addKeywordDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("AddModeratedKeyword called with invalid model state");
                    return BadRequest(new { success = false, message = "Invalid input data", errors = ModelState });
                }

                _logger.LogInformation("AddModeratedKeyword endpoint called for keyword: {Keyword}", addKeywordDto.Keyword);
                var result = await _moderatedKeywordService.AddModeratedKeywordAsync(addKeywordDto.Keyword);
                
                _logger.LogInformation("AddModeratedKeyword completed successfully for keyword: {Keyword}", addKeywordDto.Keyword);
                return Ok(new
                {
                    success = true,
                    data = result,
                    message = "Moderated keyword added successfully"
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("AddModeratedKeyword validation error: {Message}", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning("AddModeratedKeyword conflict: {Message}", ex.Message);
                return Conflict(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "AddModeratedKeyword failed for keyword {Keyword}: {Message}", addKeywordDto.Keyword, ex.Message);
                return StatusCode(500, new { success = false, message = "An error occurred while adding the keyword" });
            }
        }
    }
}