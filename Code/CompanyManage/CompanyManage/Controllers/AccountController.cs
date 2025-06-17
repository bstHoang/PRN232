using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CompanyManage.Models;

namespace CompanyManage.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() => View("~/Views/Login.cshtml");

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserNameOrEmail) ??
                           await _userManager.FindByEmailAsync(model.UserNameOrEmail);

                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Account does not exist.");
                    return View("~/Views/Login.cshtml");
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                        return RedirectToAction("Index", "HomeAdmin");
                    return RedirectToAction("Index", "UserHome");
                }

                if (result.IsLockedOut)
                {
                    ModelState.AddModelError(string.Empty, "Account is locked out.");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid password.");
                }
            }

            return View("~/Views/Login.cshtml");
        }
    }

}