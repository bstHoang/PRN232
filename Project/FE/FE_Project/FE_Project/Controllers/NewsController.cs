using FE_Project.Models;
using FE_Project.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FE_Project.Controllers
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
        [Authorize(Roles = "Journalist")]
        public IActionResult Create() => View();

        [HttpPost]
        [Authorize(Roles = "Journalist")]
        public async Task<IActionResult> Create(CreateNewsModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var token = HttpContext.Session.GetString("Token");
            var response = await _apiService.CreateNews(model, token);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("", "Failed to create news.");
            return View(model);
        }

        // My News
        [Authorize(Roles = "Journalist")]
        public async Task<IActionResult> MyNews()
        {
            var token = HttpContext.Session.GetString("Token");
            var news = await _apiService.GetMyNews(token);
            return View(news);
        }

        // Update News
        [Authorize(Roles = "Journalist,Manager")]
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
        [Authorize(Roles = "Journalist,Manager")]
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
        [Authorize(Roles = "Journalist,Manager")]
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