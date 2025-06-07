using LAB1.DTOs;
using LAB1.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace LAB1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<NewsDto>>> Get()
        {
            var news = await _newsService.GetAllNewsAsync();
            return Ok(news);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsDto>> Get(int id)
        {
            try
            {
                var news = await _newsService.GetNewsByIdAsync(id);
                return Ok(news);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<ActionResult<NewsDto>> Post([FromBody] NewsDto newsDto)
        {
            var createdNews = await _newsService.CreateNewsAsync(newsDto);
            return CreatedAtAction(nameof(Get), new { id = createdNews.Id }, createdNews);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<NewsDto>> Put(int id, [FromBody] NewsDto newsDto)
        {
            try
            {
                var updatedNews = await _newsService.UpdateNewsAsync(id, newsDto);
                return Ok(updatedNews);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _newsService.DeleteNewsAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
