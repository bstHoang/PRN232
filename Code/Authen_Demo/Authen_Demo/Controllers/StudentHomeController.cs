using Authen_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authen_Demo.Controllers
{
    public class StudentHomeController : Controller
    {
        private readonly AuthenDemoContext _context;
        public StudentHomeController(AuthenDemoContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var account = HttpContext.Session.GetString("Account");
            var studentList = _context.Users.Include(u => u.Role).Where(u => u.RoleId == 3).ToList();
            if (string.IsNullOrEmpty(account))
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.Account = account;
            return View("~/Views/StudentHome.cshtml", studentList);
        }
        [HttpGet]
        public IActionResult Grade()
        {
            var account = HttpContext.Session.GetString("Account");

            if (string.IsNullOrEmpty(account))
            {
                return RedirectToAction("Index", "Login");
            }

            // Tìm user tương ứng với account
            var student = _context.Users
                .Include(u => u.Grades)
                .FirstOrDefault(u => u.Account == account);

            if (student == null)
            {
                return NotFound("Not found");
            }

            var grades = _context.Grades
        .Include(g => g.Subject)
        .Include(g => g.User)
        .Where(g => g.User.Account == account)
        .ToList();

            // Lấy danh sách các môn học duy nhất
            var subjects = grades
                .Select(g => g.Subject)
                .Where(s => s != null)
                .Distinct()
                .ToList();

            // Gửi cả danh sách môn học và điểm sang View
            ViewBag.Subjects = subjects;
            ViewBag.AllGrades = grades;


            return View("~/Views/StudentGrade.cshtml", student);
        }
        [HttpGet]
        public IActionResult SubjectGrade(int id)
        {
            var account = HttpContext.Session.GetString("Account");

            if (string.IsNullOrEmpty(account))
                return RedirectToAction("Index", "Login");

            var student = _context.Users
                .Include(u => u.Grades)
                    .ThenInclude(g => g.Subject)
                .FirstOrDefault(u => u.Account == account);

            if (student == null)
                return NotFound();

            // Lấy điểm của môn học cụ thể (SubjectId = id)
            var gradesOfSubject = student.Grades
                .Where(g => g.SubjectId == id)
                .ToList();

            var subjectName = gradesOfSubject.FirstOrDefault()?.Subject?.Name ?? "Không xác định";

            ViewBag.SubjectName = subjectName;

            return View("~/Views/SubjectGrade.cshtml", gradesOfSubject);
        }

    }
}
