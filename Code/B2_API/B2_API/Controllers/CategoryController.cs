using B2_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace B2_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CategoryController : Controller
    {
         ProductCategoryDbContext _context;

        public CategoryController(ProductCategoryDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult List()
        {
            var categories = _context.Categories.ToList();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public IActionResult List(int id)
        {
            var categories = _context.Categories.FirstOrDefault(p => p.Id == id);
            if (categories == null)
                return NotFound();
            return Ok(categories);
        }
    }
}
