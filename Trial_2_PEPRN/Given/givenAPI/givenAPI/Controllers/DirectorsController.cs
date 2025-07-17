using Microsoft.AspNetCore.Mvc;
using givenAPI.Models;

namespace givenAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DirectorsController : ControllerBase
    {
        // GET: api/Directors/GetDirectors
        [HttpGet("GetDirectors")]
        public IActionResult GetDirectors()
        {
            var directors = DataInitializer.Directors;
            return Ok(directors);
        }
    }
}

