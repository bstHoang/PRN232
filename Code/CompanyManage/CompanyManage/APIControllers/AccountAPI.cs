using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using CompanyManage.Models;

namespace CompanyManage.APIControllers
{
    public class AccountAPI : ControllerBase
    {
        private readonly CompanyDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountAPI(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            CompanyDbContext context,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("api/AccountAPI/Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.UserNameOrEmail);
                if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                    // Add roles
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    // Add custom claims (Department, Position)
                    if (user.DepartmentId != 0)
                    {
                        var department = await _context.Departments.FindAsync(user.DepartmentId);
                        if (department != null)
                            claims.Add(new Claim("Department", department.Name));
                    }
                    if (user.PositionId != 0)
                    {
                        var position = await _context.Positions.FindAsync(user.PositionId);
                        if (position != null)
                            claims.Add(new Claim("Position", position.Name));
                    }

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        issuer: _configuration["Jwt:Issuer"],
                        audience: _configuration["Jwt:Audience"],
                        claims: claims,
                        expires: DateTime.Now.AddHours(1),
                        signingCredentials: creds);

                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(token)
                    });
                }
                return Unauthorized("Invalid login attempt");
            }
            return BadRequest(ModelState);
        }
    }
}
