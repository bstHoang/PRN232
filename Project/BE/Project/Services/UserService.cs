using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Project.DTOs;
using Project.Interfaces;
using Project.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IEmailService emailService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task RegisterAsync(RegisterDto registerDto)
        {
            if (registerDto.Password != registerDto.ConfirmPassword)
            {
                throw new Exception("Password and confirmation password do not match.");
            }

            var normalizedEmail = registerDto.Email.ToLower().Trim();
            var existingUser = await _userManager.FindByEmailAsync(normalizedEmail);
            if (existingUser != null)
            {
                throw new Exception("Email is already registered. Please verify or use a different email.");
            }

            var user = _mapper.Map<ApplicationUser>(registerDto);
            Console.WriteLine($"Before serialize - Email: {user.Email}, UserName: {user.UserName}, NormalizedEmail: {user.NormalizedEmail}, NormalizedUserName: {user.NormalizedUserName}");

            var code = _emailService.GenerateVerificationCode();

            var session = _httpContextAccessor.HttpContext.Session;
            session.Remove($"VerificationCode_{normalizedEmail}");
            session.Remove($"User_{normalizedEmail}");
            session.Remove($"Password_{normalizedEmail}");

            session.SetString($"VerificationCode_{normalizedEmail}", code);
            session.SetString($"User_{normalizedEmail}", System.Text.Json.JsonSerializer.Serialize(user));
            session.SetString($"Password_{normalizedEmail}", registerDto.Password);

            await _emailService.SendVerificationEmailAsync(registerDto.Email, code);
        }

        public async Task<IdentityResult> VerifyAndCompleteRegistrationAsync(VerifyCodeDto verifyCodeDto)
        {
            var normalizedEmail = verifyCodeDto.Email.ToLower().Trim();
            var session = _httpContextAccessor.HttpContext.Session;
            var storedCode = session.GetString($"VerificationCode_{normalizedEmail}");
            var storedUserJson = session.GetString($"User_{normalizedEmail}");
            var storedPassword = session.GetString($"Password_{normalizedEmail}");

            if (storedCode == null || storedCode != verifyCodeDto.Code || storedUserJson == null || storedPassword == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid verification code." });
            }

            var existingUser = await _userManager.FindByEmailAsync(normalizedEmail);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Email is already registered." });
            }

            var user = System.Text.Json.JsonSerializer.Deserialize<ApplicationUser>(storedUserJson);
            Console.WriteLine($"After deserialize - Email: {user.Email}, UserName: {user.UserName}, NormalizedEmail: {user.NormalizedEmail}, NormalizedUserName: {user.NormalizedUserName}");
            // Log trước khi tạo người dùng
            Console.WriteLine($"Before create - Email: {user.Email}, UserName: {user.UserName}, NormalizedEmail: {user.NormalizedEmail}, NormalizedUserName: {user.NormalizedUserName}");


            var result = await _userManager.CreateAsync(user, storedPassword);

            if (result.Succeeded)
            {
                var role = await _roleManager.FindByIdAsync("2");
                if (role == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Role with Id = 2 does not exist." });
                }

                user.RoleId = 2;
                user.EmailConfirmed = true;

                await _userManager.UpdateAsync(user);

                session.Remove($"VerificationCode_{normalizedEmail}");
                session.Remove($"User_{normalizedEmail}");
                session.Remove($"Password_{normalizedEmail}");
            }

            return result;
        }

        public async Task ResendCodeAsync(string email)
        {
            var normalizedEmail = email.ToLower().Trim();
            var existingUser = await _userManager.FindByEmailAsync(normalizedEmail);
            if (existingUser != null)
            {
                throw new Exception("Email is already registered. Please verify or use a different email.");
            }

            var session = _httpContextAccessor.HttpContext.Session;
            var storedUserJson = session.GetString($"User_{normalizedEmail}");
            var storedPassword = session.GetString($"Password_{normalizedEmail}");

            if (storedUserJson == null || storedPassword == null)
            {
                throw new Exception("No pending registration found for this email.");
            }

            var code = _emailService.GenerateVerificationCode();
            session.SetString($"VerificationCode_{normalizedEmail}", code);

            await _emailService.SendVerificationEmailAsync(email, code);
        }

        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var normalizedEmail = loginDto.Email.ToUpper().Trim();
            var user = await _userManager.FindByEmailAsync(normalizedEmail);
            if (user == null)
            {
                throw new Exception("Email not found.");
            }
            if (!await _userManager.CheckPasswordAsync(user, loginDto.Password))
            {
                throw new Exception("Password is incorrect.");
            }

            // Tạo claims cho JWT
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("RoleId", user.RoleId.ToString())
            };

            // Tạo JWT
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Token hết hạn sau 1 giờ
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}