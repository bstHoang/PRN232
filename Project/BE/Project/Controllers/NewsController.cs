using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Project.DTOs.News;
using Project.Interfaces;
using Project.Services;

namespace Project.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;

        public NewsController(INewsService newsService , ICategoryService categoryService)
        {
            _newsService = newsService;
            _categoryService =  categoryService; 
        }

        [HttpGet]
        [EnableQuery]
        [Route("api/news/getnews")]
        public async Task<IActionResult> Get()
        {
            var news = await _newsService.GetAllNewsAsync();
            return Ok(news);
        }

        [HttpGet]
        [Authorize(Roles = "JOURNALIST")]
        [Route("api/news/mynews")]
        public async Task<IActionResult> GetMyNews()
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var news = await _newsService.GetNewsByJournalistAsync(userId);
            return Ok(news);
        }

        [HttpGet]
        [Route("api/news/getnew/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var news = await _newsService.GetNewsByIdAsync(id, User);
            return Ok(news);
        }

        [HttpPost]
        [Authorize(Roles = "JOURNALIST")]
        [Route("api/news/createnew")]
        public async Task<IActionResult> Create([FromBody] NewsCreateDto newsDto)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var news = await _newsService.CreateNewsAsync(newsDto, userId);
            return CreatedAtAction(nameof(GetById), new { id = news.Id }, news);
        }

        [HttpPut]
        [Authorize(Roles = "JOURNALIST,MANAGER")]
        [Route("api/news/updatenew/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] NewsUpdateDto newsDto)
        {
            Console.WriteLine($"NewsController.Update - Updating news with Id: {id}");

            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return BadRequest("Invalid user ID.");
            }

            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            try
            {
                await _newsService.UpdateNewsAsync(id, newsDto, userId, role);

                return Ok(new { message = "News updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete]
        [Authorize(Roles = "JOURNALIST,MANAGER")]
        [Route("api/news/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value; // <== Lấy từ claim "role" đúng như JWT
            await _newsService.DeleteNewsAsync(id, userId, role);
            return NoContent();
        }

        [HttpGet]
        [EnableQuery]
        [Route("api/news/search")]
        public async Task<IActionResult> SearchByTitle([FromQuery] string title)
        {
            Console.WriteLine($"NewsController.SearchByTitle - Title: {title}");
            var news = await _newsService.SearchNewsByTitleAsync(title);
            return Ok(news);
        }

        [HttpGet]
        [Authorize(Roles = "MANAGER")]
        [Route("api/news/all")]
        public async Task<IActionResult> GetAllNewsForManager()
        {
            var news = await _newsService.GetAllNewsForManagerAsync();
            return Ok(news);
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("api/news/bycategoryid/{categoryId}")]
        public async Task<IActionResult> NewsByCategory(int categoryId)
        {
            var news = await _newsService.GetNewsByCategoryAsync(categoryId);
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(news);
        }
    }
}