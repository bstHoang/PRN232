using CompanyManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            return View("~/Views/CreateEmployee.cshtml",model);
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var model = new EditEmployeeModel
            {
                Id = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                DepartmentId = user.DepartmentId, PositionId = user.PositionId };
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id);
                if (user == null) return NotFound();
                user.Name = model.Name;
                user.Email = model.Email;
                user.DepartmentId = model.DepartmentId;
                user.PositionId = model.PositionId;
                await _userManager.UpdateAsync(user);
                return RedirectToAction("ViewEmployeeList");
            }
            ViewBag.Departments = _context.Departments.ToList();
            ViewBag.Positions = _context.Positions.ToList();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DisableEmployee(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            user.LockoutEnd = DateTimeOffset.MaxValue; // Vô hiệu hóa vĩnh viễn
            await _userManager.UpdateAsync(user);
            return RedirectToAction("ViewEmployeeList");
        }
        [Authorize(Policy = "ViewEmployeeListPolicy")]
        public IActionResult ViewEmployeeList()
        {
            var employees = _context.Users.Include(u => u.Department).
                Include(u => u.Position).ToList();
            return View(employees);
        }
    }
}
