using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using Q2.Models;

namespace Q2.Controllers
{
    public class ServicesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ServicesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string searchName)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = Utilities.GetAbsoluteUrl("api/Services");
            var url = baseUrl;

            if (!string.IsNullOrEmpty(searchName))
            {
                url += $"?$expand=Employee,Room&$filter=contains(tolower(Employee/Name), '{searchName.ToLower()}')";
                //url += $"?$expand=Employee,Room&$filter=contains(tolower(RoomTitle), '{searchName.ToLower()}')";
                //url += $"?$expand=Employee,Room&$filter=contains(tolower(FeeType), '{searchName.ToLower()}')";
            }
            else
            {
                url += "?$expand=Employee,Room";
            }

            try
            {
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var services = JsonSerializer.Deserialize<List<ServiceViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (services == null || !services.Any())
                {
                    ViewBag.Error = "Danh sách dịch vụ rỗng.";
                    return View(new List<ServiceViewModel>());
                }

                ViewBag.SearchName = searchName; 
                return View(services);
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không thể tải dữ liệu từ API: " + ex.Message;
                return View(new List<ServiceViewModel>());
            }
        }
    }
}