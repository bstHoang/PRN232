using FE_Project.Services;
using Microsoft.AspNetCore.Mvc;

namespace FE_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string title)
        {
            var news = string.IsNullOrEmpty(title) ? await _apiService.GetAllNews() : await _apiService.SearchNews(title);
            return View(news);
        }
    }
}