using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Project.DTOs;
using Project.Interfaces;
using Project.Models;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace Project.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            IMapper mapper,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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
            var code = GenerateVerificationCode();

            var session = _httpContextAccessor.HttpContext.Session;
            session.Remove($"VerificationCode_{normalizedEmail}");
            session.Remove($"User_{normalizedEmail}");
            session.Remove($"Password_{normalizedEmail}");

            session.SetString($"VerificationCode_{normalizedEmail}", code);
            session.SetString($"User_{normalizedEmail}", System.Text.Json.JsonSerializer.Serialize(user));
            session.SetString($"Password_{normalizedEmail}", registerDto.Password);

            await SendVerificationEmail(registerDto.Email, code);
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
            var result = await _userManager.CreateAsync(user, storedPassword);

            if (result.Succeeded)
            {
                var role = await _roleManager.FindByIdAsync("2");
                if (role == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "Role with Id = 2 does not exist." });
                }

                user.RoleId = 2;
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

            var code = GenerateVerificationCode();
            session.SetString($"VerificationCode_{normalizedEmail}", code);

            await SendVerificationEmail(email, code);
        }

        private string GenerateVerificationCode()
        {
            return new Random().Next(100000, 999999).ToString();
        }

        private async Task SendVerificationEmail(string email, string code)
        {
            var smtpClient = new SmtpClient(_configuration["SmtpSettings:Server"])
            {
                Port = int.Parse(_configuration["SmtpSettings:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["SmtpSettings:SenderEmail"],
                    _configuration["SmtpSettings:SenderPassword"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["SmtpSettings:SenderEmail"]),
                Subject = "Verification Code",
                Body = $"Your verification code is: {code}",
                To = { new MailAddress(email) }
            };

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}