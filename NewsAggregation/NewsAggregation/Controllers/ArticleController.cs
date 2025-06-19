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

        [HttpPost("Sync")]
        public async Task<IActionResult> SyncNews()
        {
            try
            {
                return Ok(await _articleService.SyncNews());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetNews")]
        public async Task<IActionResult> GetNews([FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string category)
        {
            // Two filters
            // Date Range and Category
            // Date Range Filter to show the news
            try
            {
                return Ok(await _articleService.GetNews(startDate, endDate, category));
            }
            catch (Exception ex)
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
        //[HttpDelete("DeleteArticle")]
        //public async Task<IActionResult> DeleteSavedArticle([FromQuery] int userId)
        //{
        //    try
        //    { }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}
