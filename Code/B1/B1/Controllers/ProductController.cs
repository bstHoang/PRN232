using B1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B1.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductCategoryDbContext _context;

        public ProductController(ProductCategoryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult List(int? categoryId)
        {
            // Lấy danh sách tất cả danh mục
            var categories = _context.Categories.ToList();

            // Nếu categoryId được cung cấp, lấy danh sách sản phẩm thuộc danh mục đó
            if (categoryId.HasValue)
            {
                var products = _context.Categories
                    .Where(c => c.Id == categoryId.Value)
                    .SelectMany(c => c.Products)
                    .ToList();



                // Truyền danh sách sản phẩm và categoryId đã chọn vào ViewBag
                ViewBag.Products = products;
            }

            // Trả về view với danh sách danh mục làm model
            return View("~/Views/Product/List.cshtml", categories);
        }
    }
}