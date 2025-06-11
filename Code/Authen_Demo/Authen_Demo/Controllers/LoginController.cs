using Authen_Demo.Models;
using Microsoft.AspNetCore.Mvc;

namespace Authen_Demo.Controllers
{
    public class LoginController : Controller
    {
        private readonly AuthenDemoContext _context;
        public LoginController(AuthenDemoContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/Login.cshtml");
        }
        [HttpPost]
        public IActionResult Index(User user)
        {
            Console.WriteLine($"[DEBUG] Account: {user.Account}, Password: {user.Password}");
            if (!ModelState.IsValid)
            {
                foreach (var modelState in ModelState)
                {
                    var propertyName = modelState.Key;
                    var errors = modelState.Value.Errors;
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"Error in {propertyName}: {error.ErrorMessage}");
                    }
                }
                return View("~/Views/Login.cshtml", user);
            }
            var checkUser = _context.Users
                .FirstOrDefault(u => u.Account == user.Account && u.Password == user.Password);

            if (checkUser == null)
            {
                ViewData["ErrorMessage"] = "Tài khoản hoặc mật khẩu không đúng";
                return View("~/Views/Login.cshtml", user);
            }
            HttpContext.Session.SetString("Account", checkUser.Account);
            HttpContext.Session.SetInt32("RoleId", checkUser.RoleId ?? 0);

            // 
            switch (checkUser.RoleId)
            {
                case 1:
                    return RedirectToAction("Index", "Home");
                case 2:
                    return RedirectToAction("Index", "TeacherHome");
                case 3:
                    return RedirectToAction("Index", "StudentHome");
                default:
                    return RedirectToAction("Index", "StudentHome");
            }
        }
    }
}