using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Project.Interfaces;

namespace Project.Controllers
{
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [EnableQuery]
        [AllowAnonymous]
        [Route("api/Categories/GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            Console.WriteLine("CategoryController.GetAllCategories - Retrieving categories");
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }
    }
}