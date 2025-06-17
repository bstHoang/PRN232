using CompanyManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManage.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ProfileController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [Authorize(Policy = "ViewProfilePolicy")]
        public async Task<IActionResult> ViewProfile(string id = null)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var isHR = User.IsInRole("User") && User.HasClaim("Department", "HR");
            var user = string.IsNullOrEmpty(id) 
                || (!isHR && id != currentUser.Id.ToString()) ? 
                currentUser : await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }
    }
}
