using Microsoft.AspNetCore.Mvc;
using NewsAggregation.Models.DTO;
using NewsAggregation.Services.Interfaces;

namespace NewsAggregation.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ArticleController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ILogger<ArticleController> _logger;
        
        public ArticleController(IArticleService articleService, ILogger<ArticleController> logger)
        {
            _articleService = articleService;
            _logger = logger;
        }


        [HttpGet("GetNews")]
        public async Task<IActionResult> GetNews([FromQuery] string? startDate = null, [FromQuery] string? endDate = null, [FromQuery] string? category = null)
        {
            try
            {
                _logger.LogInformation("GetNews request - StartDate: {StartDate}, EndDate: {EndDate}, Category: {Category}", startDate, endDate, category);
                
                startDate ??= DateTime.Today.ToString("yyyy-MM-dd");
                endDate ??= DateTime.Today.ToString("yyyy-MM-dd");

                var result = await _articleService.GetNews(startDate, endDate, category);
                
                _logger.LogInformation("GetNews completed successfully - Found {Count} articles", result.Count);
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("GetNews validation error: {Message}", ex.Message);
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetNews failed with error: {Message}", ex.Message);
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request" });
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchArticles([FromQuery] ArticleSearchRequest request)
        {
            try
            {
                _logger.LogInformation("SearchArticles request - Keyword: {Keyword}", request.Keyword);
                var articles = await _articleService.SearchArticlesAsync(request);
                _logger.LogInformation("SearchArticles completed - Found {Count} articles", articles.Count);
                return Ok(articles);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning("SearchArticles validation error: {Message}", ex.Message);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SearchArticles failed: {Message}", ex.Message);
                return StatusCode(500, "An error occurred while searching articles");
            }
        }

        [HttpGet("GetSavedArticles")]
        public async Task<IActionResult> GetSavedArticles([FromQuery] int userId)
        {
            try
            {
                _logger.LogInformation("GetSavedArticles request for UserId: {UserId}", userId);
                var result = await _articleService.GetSavedArticles(userId);
                _logger.LogInformation("GetSavedArticles completed - Found {Count} saved articles", result.Count);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetSavedArticles failed for UserId {UserId}: {Message}", userId, ex.Message);
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveArticle")]
        public async Task<IActionResult> SaveArticle([FromBody] SavedArticleDto savedArticleDto)
        {
            try
            {
                _logger.LogInformation("SaveArticle request - UserId: {UserId}, ArticleId: {ArticleId}", savedArticleDto.UserId, savedArticleDto.ArticleId);
                var result = await _articleService.SaveUserArticle(savedArticleDto.UserId, savedArticleDto.ArticleId);
                _logger.LogInformation("SaveArticle completed successfully for UserId: {UserId}", savedArticleDto.UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "SaveArticle failed for UserId {UserId}, ArticleId {ArticleId}: {Message}", savedArticleDto.UserId, savedArticleDto.ArticleId, ex.Message);
                return BadRequest(ex.Message);
            }
        }
    }
}
