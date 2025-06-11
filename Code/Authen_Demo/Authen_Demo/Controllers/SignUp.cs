using Authen_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authen_Demo.Controllers
{
    public class SignUp : Controller
    {
        private readonly AuthenDemoContext _context;
        public SignUp(AuthenDemoContext context)
        {
            _context = context;
        }
        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Views/SignUp.cshtml");
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
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
                return View("~/Views/SignUp.cshtml", user);
            }

            var exists = _context.Users.FirstOrDefault(u => u.Account == user.Account);
            if (exists != null)
            {
                ModelState.AddModelError("Account", "Exist account.");
                return View("Index", user);
            }

            user.RoleId = 3; 
            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Success"] = "Signup successful! Back to login.";
            return RedirectToAction("Index", "Login");
        }
    }
}