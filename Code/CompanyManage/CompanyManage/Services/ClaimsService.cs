using CompanyManage.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace CompanyManage.Services
{
    public class ClaimsService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly CompanyDbContext _context;

        public ClaimsService(UserManager<ApplicationUser> userManager, CompanyDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task AddClaimsAsync(ApplicationUser user, List<Claim> claims)
        {
            var department = await _context.Departments.FindAsync(user.DepartmentId);
            var position = await _context.Positions.FindAsync(user.PositionId);

            if (department != null)
                claims.Add(new Claim("Department", department.Name));
            if (position != null)
                claims.Add(new Claim("Position", position.Name));
        }
    }
}
