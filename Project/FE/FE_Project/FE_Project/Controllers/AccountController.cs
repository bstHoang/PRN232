using FE_Project.Models;
using FE_Project.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FE_Project.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApiService _apiService;

        public AccountController(ApiService apiService)
        {
            _apiService = apiService;
        }

        // Register
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var response = await _apiService.Register(model);
            if (response.IsSuccessStatusCode)
            {
                HttpContext.Session.SetString("Email", model.Email);
                return RedirectToAction("Verify");
            }
            ModelState.AddModelError("", "Registration failed.");
            return View(model);
        }

        // Verify Email
        public IActionResult Verify() => View();

        [HttpPost]
        public async Task<IActionResult> Verify(VerifyModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var response = await _apiService.Verify(model);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Login");
            }
            ModelState.AddModelError("", "Verification failed.");
            return View(model);
        }

        // Login
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var response = await _apiService.Login(model);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                try
                {
                    var responseData = JsonConvert.DeserializeObject<dynamic>(json);
                    if (responseData?.Token == null)
                    {
                        ModelState.AddModelError("", "Token not found in API response.");
                        return View(model);
                    }

                    string token = responseData.Token.ToString();
                    HttpContext.Session.SetString("Token", token);
                    HttpContext.Session.SetString("Email", model.Email);

                    // Decode JWT để lấy roles
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Email),
                        new Claim(ClaimTypes.NameIdentifier, jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? model.Email)
                    };

                    // Lấy tất cả role từ JWT
                    var roles = jwtToken.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();
                    if (!roles.Any())
                    {
                        ModelState.AddModelError("", "No roles found in token.");
                        return View(model);
                    }

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                    return RedirectToAction("Index", "Home");
                }
                catch (JsonException ex)
                {
                    ModelState.AddModelError("", $"Error parsing API response: {ex.Message}");
                    return View(model);
                }
                catch (SecurityTokenException ex)
                {
                    ModelState.AddModelError("", $"Error decoding JWT token: {ex.Message}");
                    return View(model);
                }
            }
            ModelState.AddModelError("", "Login failed.");
            return View(model);
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}