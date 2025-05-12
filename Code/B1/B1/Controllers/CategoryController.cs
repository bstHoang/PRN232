using B1.Models;
using Microsoft.AspNetCore.Mvc;

namespace B1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ProductCategoryDbContext _context;

        public CategoryController(ProductCategoryDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var categories = _context.Categories.ToList();
            return View("~/Views/Product/List.cshtml", categories);
        }

    }
}