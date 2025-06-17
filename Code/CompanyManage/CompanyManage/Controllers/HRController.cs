using CompanyManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManage.Controllers
{
    [Authorize(Policy = "HRManagerPolicy")]
    public class HRController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CompanyDbContext _context;

        public HRController(UserManager<ApplicationUser> userManager, CompanyDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public IActionResult CreateEmployee()
        {
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            return View("~/Views/CreateEmployee.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee(CreateEmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    DepartmentId = model.DepartmentId,
                    PositionId = model.PositionId,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");
                    return RedirectToAction("ViewEmployeeList");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            return View(model);
        }
    }
}
