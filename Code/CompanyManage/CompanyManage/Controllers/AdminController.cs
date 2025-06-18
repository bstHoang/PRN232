using CompanyManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManage.Controllers
{
    [Authorize(Policy = "AdminPolicy")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View("~/Views/CreateAccount.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View("~/Views/CreateAccount.cshtml",model);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var model = new EditRoleModel 
            { 
                UserId = user.Id.ToString(), 
                UserName = user.UserName, 
                Role = (await _userManager.GetRolesAsync(user)).FirstOrDefault() };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.UserId);
                if (user == null) return NotFound();
                var currentRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                if (currentRole != model.Role)
                {
                    if (!string.IsNullOrEmpty(currentRole)) await _userManager.RemoveFromRoleAsync(user, currentRole);
                    await _userManager.AddToRoleAsync(user, model.Role);
                }
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }
    }
}
