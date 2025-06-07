using B2_API.Mapping;
using B2_API.Models;
using B2_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace B2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ProductCategoryDbContext _context;

        public ProductsController(ProductCategoryDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> GetProductsWithCategories()
        {
            var products = await _context.Products
                .AsNoTracking() // Tối ưu hóa hiệu suất cho truy vấn chỉ đọc
                .Include(p => p.Categories) // Lấy danh sách Categories liên quan
                .ToListAsync();

            var productDTOs = products.Select(p => MappingHelper.ToProductDTO(p)).ToList();
            return Ok(productDTOs);
        }

        // GET: api/Products/Categories
        // Lấy danh sách danh mục cùng các sản phẩm thuộc từng danh mục
        [HttpGet("Categories")]
        public async Task<ActionResult<IEnumerable<CategoryWithProductsDTO>>> GetCategoriesWithProducts()
        {
            var categories = await _context.Categories
                .AsNoTracking() // Tối ưu hóa hiệu suất
                .Include(c => c.Products) // Lấy danh sách Products liên quan
                .ToListAsync();

            var categoryDTOs = categories.Select(c => MappingHelper.ToCategoryWithProductsDTO(c)).ToList();
            return Ok(categoryDTOs);
        }
    }

}
