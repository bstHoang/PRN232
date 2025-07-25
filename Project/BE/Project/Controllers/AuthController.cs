using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Project.DTOs;
using Project.Interfaces;
using Project.Services;

namespace Project.Controllers
{
   
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("api/auth/register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await _userService.RegisterAsync(registerDto);
                return Ok(new { Message = "Verification code sent to email." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/auth/verify")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto verifyCodeDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.VerifyAndCompleteRegistrationAsync(verifyCodeDto);
            if (result.Succeeded)
            {
                return Ok(new { Message = "Registration completed successfully." });
            }

            return BadRequest(result.Errors);
        }
        [HttpPost]
        [Route("api/auth/resendcode")]
        public async Task<IActionResult> ResendCode([FromBody] EmailRequest request)
        {
            if (string.IsNullOrEmpty(request.Email))
            {
                return BadRequest(new { Message = "Email is required." });
            }

            try
            {
                await _userService.ResendCodeAsync(request.Email);
                return Ok(new { Message = "Verification code resent to email." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var token = await _userService.LoginAsync(loginDto);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [EnableQuery]
        [AllowAnonymous]
        [Route("api/auth/GetAllAccounts")]
        public async Task<IActionResult> GetAllAccounts()
        {
            Console.WriteLine("AccountController.GetAllAccounts - Retrieving accounts");
            var accounts = await _userService.GetAllAccountsAsync();
            return Ok(accounts);
        }
        [HttpGet]
        [EnableQuery]
        [AllowAnonymous]
        [Route("api/auth/GetAccounts/{id}")]
        public async Task<IActionResult> GetAccountById(string id)
        {
            Console.WriteLine($"AccountController.GetAccountById - Retrieving account with Id: {id}");
            try
            {
                var account = await _userService.GetAccountByIdAsync(id);
                return Ok(account);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AccountController.GetAccountById - Error: {ex.Message}");
                return NotFound(ex.Message);
            }
        }
    }
}
