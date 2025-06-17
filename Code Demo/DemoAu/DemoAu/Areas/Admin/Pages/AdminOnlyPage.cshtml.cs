using DemoAu.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DemoAu.Areas.Admin.Pages
{
    [Authorize(Roles = "ADMIN")]
    public class AdminOnlyPageModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        public AdminOnlyPageModel(UserManager<AppUser> userManager)
        {
            _userManager= userManager;
        }
        public void OnGet()
        {
            List<AppUser> users= _userManager.Users.ToList();
            
        }
    }
}
