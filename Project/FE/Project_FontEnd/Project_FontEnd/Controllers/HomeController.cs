using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_FontEnd.Models;
using Project_FontEnd.Services;
using System.Diagnostics;

namespace Project_FontEnd.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApiService _apiService;

        public HomeController(ApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index(string? title, int? categoryId)
        {
            var categories = await _apiService.GetAllCategories();
            ViewBag.Categories = categories;

            List<NewsModel> newsList;

            if (categoryId.HasValue)
            {
                newsList = await _apiService.GetNewsByCategoryId(categoryId.Value);
                ViewBag.SelectedCategoryId = categoryId.Value;
            }
            else if (!string.IsNullOrEmpty(title))
            {
                ViewBag.SearchTitle = title;
                newsList = await _apiService.SearchNews(title);
            }
            else
            {
                newsList = await _apiService.GetAllNews();
            }

            return View(newsList);
        }


        [AllowAnonymous]
        public async Task<IActionResult> NewsByCategory(int id)
        {
            var categories = await _apiService.GetAllCategories();
            var newsList = await _apiService.GetNewsByCategoryId(id);

            ViewBag.Categories = categories;
            ViewBag.SelectedCategoryId = id;
            return View("Index", newsList); 
        }
    }
}
