using CompanyManage.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

namespace CompanyManage.APIControllers
{
    public class AdminAPI : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CompanyDbContext _context;
        public AdminAPI(UserManager<ApplicationUser> userManager, CompanyDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        [Authorize(Policy = "ViewEmployeeListPolicy")]
        [HttpGet]
        [Route("api/AdminAPI/ViewEmployeeList")]
        [EnableQuery]
        public async Task<IActionResult> ViewEmployeeList()
        {
            var users = await _context.Users
                .Include(u => u.Department)
                .Include(u => u.Position)
                .ToListAsync();

            var result = new List<object>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user); // Phải gọi ở đây

                result.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    user.Name,
                    Department = user.Department?.Name ?? "N/A",
                    Position = user.Position?.Name ?? "N/A",
                    IsDisabled = user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow,
                    Roles = roles // Trả về danh sách Role (dạng List<string>)
                });
            }

            return Ok(result);
        }
        [Authorize(Policy = "ViewEmployeeListPolicy")]
        [HttpGet]
        [Route("api/AdminAPI/ViewEmployeeList/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userManager.Users
                .Include(u => u.Department)
                .Include(u => u.Position)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return NotFound("User not found");

            var roles = await _userManager.GetRolesAsync(user);

            var result = new
            {
                user.Id,
                user.Name,
                user.Email,
                Department = user.Department?.Name ?? "N/A",
                Position = user.Position?.Name ?? "N/A",
                IsDisabled = user.LockoutEnd != null && user.LockoutEnd > DateTimeOffset.UtcNow,
                Role = roles.FirstOrDefault() ?? "" // Nếu chỉ có 1 role
            };

            return Ok(result);
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut]
        [Route("api/AdminAPI/CreateAccount/{id}")]
        public async Task<IActionResult> CreateAccount(int id, [FromBody] CreateEmployeeModel model)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound();

            // Gán email nếu chưa có
            if (string.IsNullOrEmpty(user.Email) && !string.IsNullOrEmpty(model.Email))
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                    return BadRequest(setEmailResult.Errors);

                user.EmailConfirmed = true;
            }

            // Gán mật khẩu nếu user chưa có (User chưa có mật khẩu => PasswordHash = null)
            if (string.IsNullOrEmpty(user.PasswordHash) && !string.IsNullOrEmpty(model.Password))
            {
                var addPwdResult = await _userManager.AddPasswordAsync(user, model.Password);
                if (!addPwdResult.Succeeded)
                    return BadRequest(addPwdResult.Errors);
            }

            // Cập nhật lại thông tin user
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Đã cập nhật email, mật khẩu và xác nhận email thành công." });
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpPut]
        [Route("api/AdminAPI/UpdateAccountInfo/{id}")]
        public async Task<IActionResult> UpdateAccountInfo(int id, [FromBody] CreateEmployeeModel model)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(new { Message = "Không tìm thấy người dùng." });

            // Cập nhật email
            if (!string.IsNullOrEmpty(model.Email))
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, model.Email);
                if (!setEmailResult.Succeeded)
                    return BadRequest(setEmailResult.Errors);
                user.EmailConfirmed = true;
            }

            // Cập nhật mật khẩu
            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetResult = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!resetResult.Succeeded)
                    return BadRequest(resetResult.Errors);
            }   
            // Khóa tài khoản (Disable)
            if (model.IsDisabled)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.MaxValue;
            }
            else
            {
                user.LockoutEnd = null;
                user.LockoutEnabled = false;
            }

            // Cập nhật Role (nếu có)
            if (!string.IsNullOrEmpty(model.RoleName))
            {
                var currentRoles = await _userManager.GetRolesAsync(user);

                // Xóa các role hiện tại
                var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeResult.Succeeded)
                    return BadRequest(removeResult.Errors);

                // Gán Role mới
                var addResult = await _userManager.AddToRoleAsync(user, model.RoleName);
                if (!addResult.Succeeded)
                    return BadRequest(addResult.Errors);
            }

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Đã cập nhật phòng ban, chức vụ và khóa tài khoản thành công." });
            }

            return BadRequest(result.Errors);
        }

        [Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        [Route("api/AdminAPI/GetUserRoles/{id}")]
        public async Task<IActionResult> GetUserRoles(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
                return NotFound(new { Message = "Không tìm thấy người dùng." });

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }
    }
}
