using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs.Roles;
using Project.Interfaces;

namespace Project.Controllers
{
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        [Route("api/Categories/updaterole")]
        public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleDto dto)
        {
            var result = await _roleService.UpdateUserRoleAsync(dto);
            if (!result)
                return BadRequest("Cập nhật vai trò thất bại.");

            return Ok("Cập nhật vai trò thành công.");
        }
    }
}