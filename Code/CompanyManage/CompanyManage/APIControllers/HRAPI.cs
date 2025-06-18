using CompanyManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompanyManage.APIControllers
{
    public class HRAPI : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CompanyDbContext _context;

        public HRAPI(UserManager<ApplicationUser> userManager, CompanyDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Policy = "ViewEmployeeListPolicy")]
        [HttpGet]
        [Route("api/HRAPI/ViewEmployeeList")]
        public IActionResult ViewEmployeeList()
        {
            var employees = _context.Users
                .Include(u => u.Department)
                .Include(u => u.Position)
                .Select(u => new
                {
                    u.Id,
                    u.UserName,
                    u.Email,
                    u.Name,
                    Department = u.Department != null ? u.Department.Name : "N/A",
                    Position = u.Position != null ? u.Position.Name : "N/A"
                })
                .ToList();

            return Ok(employees);
        }
    }
}
