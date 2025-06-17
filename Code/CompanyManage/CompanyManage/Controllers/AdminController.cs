using Microsoft.AspNetCore.Mvc;

namespace CompanyManage.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
