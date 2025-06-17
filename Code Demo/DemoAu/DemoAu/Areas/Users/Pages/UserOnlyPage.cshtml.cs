using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoAu.Areas.Users
{
    [Authorize]
    public class UserOnlyPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
