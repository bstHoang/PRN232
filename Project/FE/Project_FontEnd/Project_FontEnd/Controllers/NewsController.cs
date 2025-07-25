using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Project_FontEnd.Models;
using Project_FontEnd.Services;
using System.Security.Claims;

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

        [Authorize(Roles = "JOURNALIST, MANAGER")]
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var news = await _apiService.GetNewsById(id);
            if (news == null)
            {
                return NotFound();
            }

            if (User.IsInRole("JOURNALIST"))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (news.CreateBy.ToString() != userId)
                {
                    return Forbid();
                }
            }

            var tagIds = await _apiService.GetTagIdsFromNames(news.Tags);
            var model = new UpdateNewsModel
            {
                Id = news.Id,
                Title = news.Title,
                Description = news.Description,
                Content = news.Content,
                CategoryId = news.CategoryId,
                TagIds = tagIds,
                Disable = news.Disable
            };

            var categories = await _apiService.GetAllCategories() ?? new List<CategoryModel>();
            var tags = await _apiService.GetAllTags() ?? new List<TagModel>();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            ViewBag.Tags = tags;
            return View(model);
        }

        [Authorize(Roles = "JOURNALIST, MANAGER")]
        [HttpPost]
        public async Task<IActionResult> Update(int id, UpdateNewsModel model)
        {
            // Bỏ qua validation cho MANAGER
            if (User.IsInRole("MANAGER"))
            {
                ModelState.Clear();
                ModelState.AddModelError("", "");
            }
            // Chỉ kiểm tra validation cho JOURNALIST
            else if (!ModelState.IsValid)
            {
                var categories = await _apiService.GetAllCategories() ?? new List<CategoryModel>();
                var tags = await _apiService.GetAllTags() ?? new List<TagModel>();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                ViewBag.Tags = tags;
                return View(model);
            }

            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                ModelState.AddModelError("", "Please login again.");
                var categories = await _apiService.GetAllCategories() ?? new List<CategoryModel>();
                var tags = await _apiService.GetAllTags() ?? new List<TagModel>();
                ViewBag.Categories = new SelectList(categories, "Id", "Name");
                ViewBag.Tags = tags;
                return View(model);
            }

            var news = await _apiService.GetNewsById(id);
            if (news == null)
            {
                return NotFound();
            }

            if (User.IsInRole("JOURNALIST"))
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (news.CreateBy.ToString() != userId)
                {
                    return Forbid();
                }

                var journalistModel = new UpdateNewsModel
                {
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    CategoryId = model.CategoryId,
                    TagIds = model.TagIds
                };

                var response = await _apiService.UpdateNews(id, journalistModel, token);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật bài viết thành công."; // ✅ Thêm thông báo
                    return RedirectToAction("MyNews");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<dynamic>(errorContent);
                    var errorMessage = errorObj?.message?.ToString() ?? $"Failed to update news: HTTP {response.StatusCode}";
                    ModelState.AddModelError("", errorMessage);
                }
                catch
                {
                    ModelState.AddModelError("", $"Failed to update news: HTTP {response.StatusCode}");
                }

                var categoriesReload = await _apiService.GetAllCategories() ?? new List<CategoryModel>();
                var tagsReload = await _apiService.GetAllTags() ?? new List<TagModel>();
                ViewBag.Categories = new SelectList(categoriesReload, "Id", "Name");
                ViewBag.Tags = tagsReload;
                return View(model);
            }
            else if (User.IsInRole("MANAGER"))
            {
                var managerModel = new UpdateNewsModel { Disable = model.Disable };
                var response = await _apiService.UpdateNews(id, managerModel, token);
                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Cập nhật trạng thái thành công."; // ✅ Thêm thông báo
                    return RedirectToAction("Index", "Home");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                try
                {
                    var errorObj = JsonConvert.DeserializeObject<dynamic>(errorContent);
                    var errorMessage = errorObj?.message?.ToString() ?? $"Failed to update news: HTTP {response.StatusCode}";
                    ModelState.AddModelError("", errorMessage);
                }
                catch
                {
                    ModelState.AddModelError("", $"Failed to update news: HTTP {response.StatusCode}");
                }

                var categoriesReload = await _apiService.GetAllCategories() ?? new List<CategoryModel>();
                var tagsReload = await _apiService.GetAllTags() ?? new List<TagModel>();
                ViewBag.Categories = new SelectList(categoriesReload, "Id", "Name");
                ViewBag.Tags = tagsReload;
                return View(model);
            }

            return Forbid();
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

        [Authorize(Roles = "JOURNALIST,MANAGER")]
        public async Task<IActionResult> DeleteManage(int id)
        {
            var token = HttpContext.Session.GetString("Token");
            var response = await _apiService.DeleteNews(id, token);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Manage", "News");
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

        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> Manage(string? title) 
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var newsList = await _apiService.GetAllNewsForManager(token, title);
            var newsList1 = string.IsNullOrEmpty(title) ? await _apiService.GetAllNewsForManager(token , title) : await _apiService.SearchNews(title);
            return View(newsList1);
        }


        [Authorize(Roles = "MANAGER")]
        [HttpGet]
        public async Task<IActionResult> ManageDetail(int id)
        {
            var news = await _apiService.GetNewsById(id);
            return View(news);
        }

        [Authorize(Roles = "MANAGER")]
        [HttpPost]
        public async Task<IActionResult> ManageDetail(NewsModel model)
        {
            var token = HttpContext.Session.GetString("Token");

            var updateData = new
            {
                disable = model.Disable
            };

            var response = await _apiService.UpdateNews(model.Id, updateData, token);
            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = "Update successful";
                return RedirectToAction("Manage");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to update news");
                return View(model);
            }
        }
       
    }
}   