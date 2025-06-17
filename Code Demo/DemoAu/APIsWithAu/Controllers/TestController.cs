using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace APIsWithAu.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TestController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("abs");
        }
        [HttpGet]
        public IActionResult List()
        {
            var jwtToken = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            // Cấu hình đối tượng SecurityTokenValidator để kiểm tra token.
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidAudience = _configuration["Jwt:Audience"],
                IssuerSigningKey = key
            };

            try
            {
                // Giải mã và kiểm tra JWT token.
                var tokenHandler = new JwtSecurityTokenHandler();
                var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, tokenValidationParameters, out var validatedToken);

                // Lấy thông tin từ token.
                var major = claimsPrincipal.FindFirst("Major")?.Value;
                var userName = claimsPrincipal.FindFirst(ClaimTypes.Name)?.Value;

                return Ok($"User Id: {major}, Username: {userName}");
            }
            catch { }

            return NoContent();
        }
    }
}
