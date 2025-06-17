using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DemoAu.Areas.Users.Pages
{
    [Authorize (Policy = "SEPolicy")]
    public class SEStudentPageModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
