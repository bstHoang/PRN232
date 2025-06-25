using AdminClient.Models;
using AdminClient.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdminClient.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApiService _apiService;

        public AdminController(ApiService apiService)
        {
            _apiService = apiService;
        }

        private IActionResult RedirectIfNotLoggedIn()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken")))
                return RedirectToAction("Login", "Account");
            return null;
        }

        public async Task<IActionResult> EmployeeList(string department, string position)
        {
            var redirect = RedirectIfNotLoggedIn();
            if (redirect != null) return redirect;

            string filter = "";
            if (!string.IsNullOrEmpty(department))
                filter += $"Department eq '{department}'";
            if (!string.IsNullOrEmpty(position))
                filter += (string.IsNullOrEmpty(filter) ? "" : " and ") + $"Position eq '{position}'";

            var employees = await _apiService.GetEmployeeListAsync(filter);
            ViewBag.Departments = await _apiService.GetDepartmentsAsync();
            ViewBag.Positions = await _apiService.GetPositionsAsync();
            return View(employees);
        }

        public async Task<IActionResult> SetCredentials(int id)
        {
            var redirect = RedirectIfNotLoggedIn();
            if (redirect != null) return redirect;

            return View(new SetCredentialsModel());
        }

        [HttpPost]
        public async Task<IActionResult> SetCredentials(int id, SetCredentialsModel model)
        {
            var redirect = RedirectIfNotLoggedIn();
            if (redirect != null) return redirect;

            if (ModelState.IsValid)
            {
                var success = await _apiService.SetCredentialsAsync(id, model);
                if (success)
                    return RedirectToAction("EmployeeList");
                ModelState.AddModelError("", "Cập nhật thất bại");
            }
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> EditDetails(int id)
        {
            var redirect = RedirectIfNotLoggedIn();
            if (redirect != null) return redirect;

            try
            {
                // GỌI API MỚI để lấy thông tin chi tiết người dùng
                var model = await _apiService.GetUserByIdAsync(id);
                Console.WriteLine("RoleName: " + model.RoleName);
                if (model == null)
                {
                    Console.WriteLine("Không tìm thấy người dùng");
                    return NotFound();
                }

                // Gán RoleName từ danh sách Roles nếu chưa có
                if (string.IsNullOrEmpty(model.RoleName))
                {
                    model.RoleName = model.Roles?.FirstOrDefault();
                }

                // LẤY DANH SÁCH ROLE để hiển thị dropdown
                ViewBag.Roles = await _apiService.GetRolesAsync() ?? new List<string>();

                return View(model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in EditDetails: {ex.Message}");
                ModelState.AddModelError("", $"Lỗi khi lấy thông tin: {ex.Message}");
                ViewBag.Roles = new List<string>();
                return View(new UpdateDetailsModel());
            }
        }



        [HttpPost]
        public async Task<IActionResult> EditDetails(int id, UpdateDetailsModel model)
        {
            foreach (var entry in ModelState)
            {
                Console.WriteLine($"Key: {entry.Key}, Errors: {string.Join(", ", entry.Value.Errors.Select(e => e.ErrorMessage))}");
            }
            var redirect = RedirectIfNotLoggedIn();
            if (redirect != null) return redirect;

            try
            {
                // Gỡ lỗi: in ra toàn bộ model
                Console.WriteLine("----- POST EditDetails -----");
                Console.WriteLine($"Email: {model.Email}");
                Console.WriteLine($"Password: {model.Password}");
                Console.WriteLine($"RoleName: {model.RoleName}");
                Console.WriteLine($"IsDisabled: {model.IsDisabled}");

                if (ModelState.IsValid)
                {
                    var success = await _apiService.UpdateAccountInfoAsync(id, model);
                    if (success)
                        return RedirectToAction("EmployeeList");

                    ModelState.AddModelError("", "Cập nhật thất bại");
                }

                ViewBag.Roles = await _apiService.GetRolesAsync() ?? new List<string>();
                return View(model);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in EditDetails POST: {ex.Message}");
                ModelState.AddModelError("", $"Lỗi khi cập nhật: {ex.Message}");
                ViewBag.Roles = await _apiService.GetRolesAsync() ?? new List<string>();
                return View(model);
            }
        }
    }
}
