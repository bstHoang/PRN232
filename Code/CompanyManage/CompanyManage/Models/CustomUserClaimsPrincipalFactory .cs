using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace CompanyManage.Models
{
    public class CustomUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole<int>>
    {
        private readonly CompanyDbContext _context;

        public CustomUserClaimsPrincipalFactory(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<int>> roleManager,
            IOptions<IdentityOptions> optionsAccessor,
            CompanyDbContext context)
            : base(userManager, roleManager, optionsAccessor)
        {
            _context = context;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            var department = await _context.Departments.FindAsync(user.DepartmentId);
            var position = await _context.Positions.FindAsync(user.PositionId);

            if (department != null)
                identity.AddClaim(new Claim("Department", department.Name));
            if (position != null)
                identity.AddClaim(new Claim("Position", position.Name));

            return identity;
        }
    }
}