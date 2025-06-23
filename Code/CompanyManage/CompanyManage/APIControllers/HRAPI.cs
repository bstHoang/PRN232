using CompanyManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
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
        [EnableQuery]
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

        [Authorize(Policy = "HRManagerPolicy")]
        [HttpPost]
        [Route("api/HRAPI/Employees")]
        public async Task<IActionResult> CreateEmployee([FromBody] CreateEmployeeModel model)
        {
            var newUser = new ApplicationUser
            {
                UserName = model.UserName,
                Name = model.Name,
                DepartmentId = model.DepartmentId,
                PositionId = model.PositionId
                //EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newUser);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Gán role có ID = 2
            var role = await _context.Roles.FindAsync(2);
            if (role == null)
                return BadRequest("Không tìm thấy Role với ID = 2");

            var roleResult = await _userManager.AddToRoleAsync(newUser, role.Name);
            if (!roleResult.Succeeded)
                return BadRequest(roleResult.Errors);

            return Ok(new { Message = "Tạo nhân viên và gán role thành công" });
        }

        [Authorize(Policy = "HRManagerPolicy")]
        [HttpPut]
        [Route("api/HRAPI/Employees/{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, [FromBody] CreateEmployeeModel model)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();

            user.UserName = model.UserName;
            user.Name = model.Name;
            user.DepartmentId = model.DepartmentId;
            user.PositionId = model.PositionId;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Cập nhật nhân viên thành công" });
            }
            return BadRequest(result.Errors);
        }

        [Authorize(Policy = "HRManagerPolicy")]
        [HttpDelete]
        [Route("api/HRAPI/Employees/{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Xóa nhân viên thành công" });
            }
            return BadRequest(result.Errors);
        }
    }
}
