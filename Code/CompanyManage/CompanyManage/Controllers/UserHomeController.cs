using CompanyManage.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManage.Controllers
{
    public class UserHomeController : Controller
    {
        private readonly CompanyDbContext _context;
        public UserHomeController(CompanyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/UserHome.cshtml");
        }
    }
}
