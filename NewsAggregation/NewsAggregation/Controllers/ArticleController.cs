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
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }


        [HttpGet("GetNews")]
        public async Task<IActionResult> GetNews([FromQuery] string? startDate = null, [FromQuery] string? endDate = null, [FromQuery] string? category = null)
        {
            try
            {
                startDate ??= DateTime.Today.ToString("yyyy-MM-dd");
                endDate ??= DateTime.Today.ToString("yyyy-MM-dd");

                var result = await _articleService.GetNews(startDate, endDate, category);
                return Ok(new
                {
                    success = true,
                    data = result
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request" });
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchArticles([FromQuery] ArticleSearchRequest request)
        {
            try
            {
                var articles = await _articleService.SearchArticlesAsync(request);
                return Ok(articles);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetSavedArticles")]
        public async Task<IActionResult> GetSavedArticles([FromQuery] int userId)
        {
            try
            {
                return Ok(await _articleService.GetSavedArticles(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveArticle")]
        public async Task<IActionResult> SaveArticle([FromBody] SavedArticleDto savedArticleDto)
        {
            try
            {
                return Ok(await _articleService.SaveUserArticle(savedArticleDto.UserId, savedArticleDto.ArticleId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
