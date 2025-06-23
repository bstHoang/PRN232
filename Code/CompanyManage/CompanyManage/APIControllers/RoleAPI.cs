using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompanyManage.APIControllers
{
    [Route("api/RoleAPI")]
    [ApiController]
    public class RoleAPI : Controller
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleAPI(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: api/RoleAPI
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleManager.Roles.Select(r => new
            {
                r.Id,
                r.Name
            }).ToList();

            return Ok(roles);
        }
    }
}

