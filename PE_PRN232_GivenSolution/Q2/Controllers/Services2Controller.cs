using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using Q2.Models;

namespace Q2.Controllers
{
    public class Services2Controller : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Services2Controller(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index(string[] searchFeeTypes)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = Utilities.GetAbsoluteUrl("api/Services");
            var url = baseUrl + "?$expand=Employee,Room";

            var response = await client.GetAsync(url);
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

            if (searchFeeTypes != null && searchFeeTypes.Any())
            {
                var filterConditions = searchFeeTypes.Select(feeType => $"FeeType eq '{feeType}'");
                url += $"&$filter={string.Join(" or ", filterConditions)}";
            }

            try
            {
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
                ViewBag.SearchFeeTypes = searchFeeTypes ?? Array.Empty<string>();
                return View(services ?? new List<ServiceViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Không thể tải dữ liệu từ API: " + ex.Message;
                ViewBag.FeeTypes = new List<string>();
                ViewBag.SearchFeeTypes = Array.Empty<string>();
                return View(new List<ServiceViewModel>());
            }
        }
    }
}