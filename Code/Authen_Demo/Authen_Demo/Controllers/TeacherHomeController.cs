using Authen_Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Authen_Demo.Controllers
{
    public class TeacherHomeController : Controller
    {
        private readonly AuthenDemoContext _context;
        public TeacherHomeController(AuthenDemoContext context)
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
            return View("~/Views/TeacherHome.cshtml", studentList);
        }
        [HttpGet]
        public IActionResult Update(int id )
        {
            var student = _context.Users.FirstOrDefault(u => u.UserId == id);
            if (student == null)
                return NotFound();

            var grades = _context.Grades
                            .Include(g => g.Subject)
                            .Include(g => g.User)
                            .Where(g => g.User.Account == student.Account)
                            .ToList();

            
            var subjects = grades
                .Select(g => g.Subject)
                .Where(s => s != null)
                .Distinct()
                .ToList();

            ViewBag.Subjects = subjects;
            ViewBag.AllGrades = grades;
            ViewBag.Account = student;

            return View("~/Views/TeacherUpdate.cshtml", grades);
        }
        [HttpGet]
        public IActionResult ViewListGrade(int subjectId, int userId)
        {
            var student = _context.Users.FirstOrDefault(u => u.UserId == userId);
            if (student == null)
                return NotFound("Không tìm thấy sinh viên");

            var grades = _context.Grades
                .Include(g => g.Subject)
                .Include(g => g.User)
                .Where(g => g.SubjectId == subjectId && g.UserId == userId)
                .ToList();

            var subjectName = grades.FirstOrDefault()?.Subject?.Name ?? "Không xác định";

            ViewBag.SubjectName = subjectName;
            ViewBag.StudentName = student.Account;

            return View("~/Views/UpdateGrate.cshtml", grades);
        }

        // Hiển thị form edit
        [HttpGet]
        public IActionResult EditGrade(int gradeId, int userId, int subjectId)
        {
            var grade = _context.Grades
                .Include(g => g.Subject)
                .Include(g => g.User)
                .FirstOrDefault(g => g.GradeId == gradeId);

            if (grade == null)
                return NotFound();

            ViewBag.StudentName = grade.User.Account;
            ViewBag.SubjectName = grade.Subject.Name;

            return View("~/Views/EditGrade.cshtml", grade);
        }

        // Xử lý khi submit form
        [HttpPost]
        public IActionResult EditGrade(Grade updatedGrade)
        {
            var grade = _context.Grades.FirstOrDefault(g => g.GradeId == updatedGrade.GradeId);
            if (grade == null)
                return NotFound();

            grade.GradeValue = updatedGrade.GradeValue;
            grade.Weight = updatedGrade.Weight;

            _context.SaveChanges();

            // Quay lại màn UpdateGrade
            return RedirectToAction("ViewListGrade", new { subjectId = updatedGrade.SubjectId, userId = updatedGrade.UserId });
        }

    }
}
