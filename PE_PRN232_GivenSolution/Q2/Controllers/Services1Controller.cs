using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using Q2.Models;

namespace Q2.Controllers
{
    public class Services1Controller : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Services1Controller(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string searchFeeType)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = Utilities.GetAbsoluteUrl("api/Services");
            var url = baseUrl + "?$expand=Employee,Room";

            if (!string.IsNullOrEmpty(searchFeeType))
            {
                url += $"&$filter=FeeType eq '{searchFeeType}'";
            }

            try
            {
                var response = await client.GetAsync(baseUrl + "?$expand=Employee,Room");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var allServices = JsonSerializer.Deserialize<List<ServiceViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                var feeTypes = allServices?
                    .Select(s => s.FeeType)
                    .Distinct()
                    .OrderBy(f => f)
                    .ToList() ?? new List<string>();

                response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                json = await response.Content.ReadAsStringAsync();
                var services = JsonSerializer.Deserialize<List<ServiceViewModel>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (services == null || !services.Any())
                {
                    ViewBag.Error = "Danh sách dịch vụ rỗng.";
                }

                ViewBag.FeeTypes = feeTypes;
                ViewBag.SearchFeeType = searchFeeType;
                return View(services ?? new List<ServiceViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không thể tải dữ liệu từ API: " + ex.Message;
                ViewBag.FeeTypes = new List<string>();
                return View(new List<ServiceViewModel>());
            }
        }
    }
}