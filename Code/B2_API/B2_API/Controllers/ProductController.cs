using B2_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2_API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : Controller
    {
        ProductCategoryDbContext _context;
        public ProductController(ProductCategoryDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult List()
        {
            var products = _context.Products.ToList();
            return Ok(products);
        }
        [HttpGet]
        public IActionResult ListProductCategory()
        {
            var products = _context.Products.Include(p => p.Categories)
                .Select(p => new ProductDto{
                    Id = p.Id,
                    Name = p.Name,
                    Categories = p.Categories.Select(c => c.Name).ToList()
                }).ToList();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public IActionResult List(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }
    }
}
