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

        public async Task<IActionResult> Index(string? title = null, int? categoryId = null, int? tagId = null)
        {
            try
            {
                var categories = await _apiService.GetAllCategories();
                var topTags = await _apiService.GetTopTagsAsync();
                ViewBag.Categories = categories ?? new List<CategoryModel>();
                ViewBag.TopTags = topTags ?? new List<TagModel1>();

                List<NewsModel> newsList;

                if (tagId.HasValue)
                {
                    newsList = await _apiService.GetNewsByTagAsync(tagId.Value);
                    var selectedTag = topTags.FirstOrDefault(t => t.TagId == tagId.Value);
                    ViewBag.SelectedTagId = tagId.Value;
                    ViewBag.SelectedTagName = selectedTag?.TagName ?? "Unknown Tag";
                }
                else if (categoryId.HasValue)
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

                return View("Index", newsList); // Rõ ràng chỉ định view "Index"
            }
            catch (Exception ex)
            {
                // Log lỗi để debug
                Console.WriteLine($"Index error: {ex.Message}");
                ViewBag.ErrorMessage = "An error occurred while loading the news.";
                return View("Index", new List<NewsModel>());
            }
        }

        [AllowAnonymous]
        public async Task<IActionResult> NewsByCategory(int id)
        {
            return await Index(title: null, categoryId: id);
        }

        [AllowAnonymous]
        public async Task<IActionResult> NewsByTag(int id)
        {
            return await Index(title: null, tagId: id);
        }
    }
}