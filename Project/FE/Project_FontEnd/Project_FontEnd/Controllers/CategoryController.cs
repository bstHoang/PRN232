using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project_FontEnd.Models;
using Project_FontEnd.Services;
using System;

namespace Project_FontEnd.Controllers
{
    [Authorize(Roles = "MANAGER")]
    public class CategoryController : Controller
    {
        private readonly ApiService _apiService;

        public CategoryController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Menu(string searchString)
        {
            var categories = await _apiService.GetAllCategoriesAsync();
            if (!string.IsNullOrEmpty(searchString))
            {
                categories = categories.Where(c => c.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            ViewData["CurrentFilter"] = searchString;
            return View("Menu", categories);
        }

        // Show create category form
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModel category)
        {
            Console.WriteLine($"Input Category: {JsonConvert.SerializeObject(category)}");
            if (ModelState.IsValid)
            {
                var (createdCategory, errorMessage) = await _apiService.CreateCategoryAsync(category);
                Console.WriteLine($"API Result: Category={(createdCategory != null ? "Success" : "Null")}, Error={errorMessage}");
                if (createdCategory != null)
                {
                    return RedirectToAction(nameof(Menu));
                }
                else
                {
                    ModelState.AddModelError("", errorMessage ?? "Lỗi không xác định khi tạo danh mục.");
                }
            }
            else
            {
                Console.WriteLine("ModelState is invalid:");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"ModelState Error: {error.ErrorMessage}");
                }
            }
            return View(category);
        }
        // Show update category form
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
                var category = await _apiService.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
        }

        // Handle update category submission
        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryModel category)
        {
            if (id != category.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _apiService.UpdateCategoryAsync(id, category);
                    return RedirectToAction(nameof(Menu)); // Sửa từ Index thành Menu
                }
                catch
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật danh mục.");
                }
            }
            return View(category);
        }

        // Handle delete category
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var message = await _apiService.DeleteCategoryAsync(id);
                TempData["Message"] = message;
            }
            catch
            {
                TempData["Message"] = "Lỗi khi xóa danh mục.";
            }
            return RedirectToAction(nameof(Menu)); // Sửa từ Index thành Menu
        }
    }
}