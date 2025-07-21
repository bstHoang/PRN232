using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Project.DTOs.News;
using Project.Interfaces;

namespace Project.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
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
        [Route("api/news/my-news")]
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
            var news = await _newsService.GetNewsByIdAsync(id);
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
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var role = User.FindFirst("RoleId")?.Value == "3" ? "MANAGER" : "JOURNALIST";
            await _newsService.UpdateNewsAsync(id, newsDto, userId, role);
            return Ok("update succesfull");
        }

        [HttpDelete]
        [Authorize(Roles = "JOURNALIST,MANAGER")]
        [Route("api/news/delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
            var role = User.FindFirst("RoleId")?.Value == "3" ? "MANAGER" : "JOURNALIST";
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
    }
}