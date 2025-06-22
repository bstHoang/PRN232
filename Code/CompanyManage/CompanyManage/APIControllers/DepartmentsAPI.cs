using CompanyManage.Models;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManage.APIControllers
{
    [Route("api/DepartmentsAPI")]
    [ApiController]
    public class DepartmentsAPI : Controller
    {
        private readonly CompanyDbContext _context;
        public DepartmentsAPI(CompanyDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetDepartments()
        {
            var departments = _context.Departments
                .Select(d => new { d.Id, d.Name })
                .ToList();
            return Ok(departments);
        }
    }
}
