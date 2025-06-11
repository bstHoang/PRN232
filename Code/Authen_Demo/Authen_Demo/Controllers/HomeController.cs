using Authen_Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authen_Demo.Controllers
{
    public class HomeController : Controller
    {
        private readonly AuthenDemoContext _context;
        public HomeController(AuthenDemoContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            var account = HttpContext.Session.GetString("Account");
            var userList = _context.Users.Include(u => u.Role).ToList();

            if (string.IsNullOrEmpty(account))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Account = account;
            return View("~/Views/HomePage.cshtml", userList);
        }
        [HttpGet]
        public IActionResult SetRole(int id)
        {
            var user = _context.Users.Include(u => u.Role).FirstOrDefault(u => u.UserId == id);
            if (user == null)
            {
                return NotFound();
            }

            ViewBag.Roles = _context.Roles.ToList();
            return View("~/Views/SetRole.cshtml", user);
        }
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Index", "Login");
        }
        [HttpPost]
        public IActionResult SetRole(User user)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.UserId == user.UserId);
            if (existingUser != null)
            {
                existingUser.RoleId = user.RoleId;
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
