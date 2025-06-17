using APIsWithAu.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace APIsWithAu.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        public AccountController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration)
        //public AccountController(IConfiguration configuration)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest login)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            //var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
            var user = await _userManager.FindByEmailAsync(login.Email); 
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password))
            {
                //them thong tin claim cho user

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, login.Email),
                    new Claim(ClaimTypes.Role, "User"),
                    new Claim(ClaimTypes.Email, login.Email),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                    new Claim("Major", "SE")
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var expiry = DateTime.Now.AddDays(Convert.ToInt32(_configuration["Jwt:ExpiryInDays"]));
                var token = new JwtSecurityToken(
                    _configuration["Jwt:Issuer"],
                    _configuration["Jwt:Audience"],
                    claims,
                    expires:expiry,
                    signingCredentials: creds
                    );

                return Ok(new LoginResponse { Successful=true, Token=new JwtSecurityTokenHandler().WriteToken(token)});
            }
            else
            {
                return BadRequest("Invalid login attempt.");
            }
        }

        // GET api/<AccountController>/5
        [HttpGet("{id}")]
        public string Register(int id)
        {
            return "value";
        }

        // POST api/<AccountController>
        [HttpDelete]
        public void Logout([FromBody] string value)
        {
        }

    }
}
