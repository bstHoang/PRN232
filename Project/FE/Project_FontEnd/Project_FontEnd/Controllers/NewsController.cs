using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Project_FontEnd.Models;
using Project_FontEnd.Services;

namespace Project_FontEnd.Controllers
{
    [Authorize]
    public class NewsController : Controller
    {
        private readonly ApiService _apiService;

        public NewsController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Create News
        [Authorize(Roles = "JOURNALIST")]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                var categories = await _apiService.GetAllCategories() ?? new List<CategoryModel>();
                var tags = await _apiService.GetAllTags() ?? new List<TagModel>();

                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                ViewBag.Tags = tags;

                return View(new CreateNewsModel());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create GET error: {ex.Message}");
                ModelState.AddModelError("", "Failed to load categories or tags.");
                ViewBag.Categories = new SelectList(new List<CategoryModel>(), "Id", "Name");
                ViewBag.Tags = new List<TagModel>();
                return View(new CreateNewsModel());
            }
        }

        [Authorize(Roles = "JOURNALIST")]
        [HttpPost]
        public async Task<IActionResult> Create(CreateNewsModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var categories = await _apiService.GetAllCategories() ?? new List<CategoryModel>();
                    var tags = await _apiService.GetAllTags() ?? new List<TagModel>();
                    ViewBag.Categories = new SelectList(categories, "Id", "Name");
                    ViewBag.Tags = tags;
                    return View(model);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Create POST validation error: {ex.Message}");
                    ModelState.AddModelError("", "Failed to load categories or tags.");
                    ViewBag.Categories = new SelectList(new List<CategoryModel>(), "Id", "Name");
                    ViewBag.Tags = new List<TagModel>();
                    return View(model);
                }
            }

            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Please login again.");
                try
                {
                    var categories = await _apiService.GetAllCategories() ?? new List<CategoryModel>();
                    var tags = await _apiService.GetAllTags() ?? new List<TagModel>();
                    ViewBag.Categories = new SelectList(categories, "Id", "Name");
                    ViewBag.Tags = tags;
                    return View(model);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Create POST token error: {ex.Message}");
                    ViewBag.Categories = new SelectList(new List<CategoryModel>(), "Id", "Name");
                    ViewBag.Tags = new List<TagModel>();
                    return View(model);
                }
            }

            var response = await _apiService.CreateNews(model, token);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            try
            {
                var errorObj = JsonConvert.DeserializeObject<dynamic>(errorContent);
                var errorMessage = errorObj?.message?.ToString() ?? $"Failed to create news: HTTP {response.StatusCode}";
                ModelState.AddModelError("", errorMessage);
            }
            catch
            {
                ModelState.AddModelError("", $"Failed to create news: HTTP {response.StatusCode}");
            }

            try
            {
                var categories = await _apiService.GetAllCategories() ?? new List<CategoryModel>();
                var tags = await _apiService.GetAllTags() ?? new List<TagModel>();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                ViewBag.Tags = tags;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Create POST reload error: {ex.Message}");
                ViewBag.Categories = new SelectList(new List<CategoryModel>(), "Id", "Name");
                ViewBag.Tags = new List<TagModel>();
            }
            return View(model);
        }

        // My News
        [Authorize(Roles = "JOURNALIST")]
        public async Task<IActionResult> MyNews()
        {
            var token = HttpContext.Session.GetString("Token");
            var news = await _apiService.GetMyNews(token);
            return View(news);
        }

        // Update News
        [Authorize(Roles = "JOURNALIST,MANAGER")]
        public async Task<IActionResult> Update(int id)
        {
            var news = await _apiService.GetNewsById(id);
            var model = new CreateNewsModel
            {
                Title = news.Title,
                Description = news.Description,
                Content = news.Content,
                CategoryId = news.CategoryId
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "JOURNALIST,MANAGER")]
        public async Task<IActionResult> Update(int id, CreateNewsModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var token = HttpContext.Session.GetString("Token");
            var response = await _apiService.UpdateNews(id, model, token);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Failed to update news.");
            return View(model);
        }

        // Delete News
        [Authorize(Roles = "JOURNALIST,MANAGER")]
        public async Task<IActionResult> Delete(int id)
        {
            var token = HttpContext.Session.GetString("Token");
            var response = await _apiService.DeleteNews(id, token);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            return BadRequest("Failed to delete news.");
        }

        // Details
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var news = await _apiService.GetNewsById(id);
            return View(news);
        }
    }
}   