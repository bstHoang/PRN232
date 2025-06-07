using HW2.DTOs;
using HW2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HW2.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class GradesController : ControllerBase
    {
        private readonly Hw1Context _context;

        public GradesController(Hw1Context context)
        {
            _context = context;
        }

        // GET: api/grades
        [HttpGet]
        public ActionResult<IEnumerable<GradeInfoDto>> GetAllGrades()
        {
            var result = _context.Grades
                .Include(g => g.Student)
                .Select(g => new GradeInfoDto
                {
                    GradeID = g.GradeId,
                    StudentID = (int)g.StudentId,
                    StudentName = g.Student.Name,
                    SubjectID = (int)g.SubjectId,
                    GradeValue = (decimal)g.GradeValue
                }).ToList();

            return Ok(result);
        }

        // GET: api/grades/{id}
        [HttpGet("{id}")]
        public ActionResult<GradeInfoDto> GetGrade(int id)
        {
            var grade = _context.Grades
                .Include(g => g.Student)
                .Where(g => g.GradeId == id)
                .Select(g => new GradeInfoDto
                {
                    GradeID = g.GradeId,
                    StudentID = (int)g.StudentId,
                    StudentName = g.Student.Name,
                    SubjectID = (int)g.SubjectId,
                    GradeValue = (decimal)g.GradeValue
                }).FirstOrDefault();

            if (grade == null) return NotFound();

            return Ok(grade);
        }
    }

}