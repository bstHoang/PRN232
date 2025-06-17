using CompanyManage.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManage.Controllers
{
    public class HomeAdminController : Controller
    {
        private readonly CompanyDbContext _context;
        public HomeAdminController(CompanyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View("~/Views/AdminHome.cshtml");
        }
    }
}
