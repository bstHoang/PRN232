using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Project_FontEnd.Models;
using Project_FontEnd.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Project_FontEnd.Controllers
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

        [HttpPost]
        public async Task<IActionResult> ResendCode()
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return Json(new { success = false, message = "Email not found in session. Please register again." });
            }

            var success = await _apiService.ResendCode(email);
            return Json(new { success, message = success ? "Code resent successfully" : "Failed to resend code" });
        }


        // Login
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("Login: ModelState invalid");
                return View(model);
            }

            var response = await _apiService.Login(model);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login: API response: {json}");
                try
                {
                    var responseData = JsonConvert.DeserializeObject<dynamic>(json);
                    string token = responseData?.Token?.ToString() ?? responseData?.token?.ToString() ?? responseData?.accessToken?.ToString();
                    if (string.IsNullOrEmpty(token))
                    {
                        string errorMessage = responseData?.message?.ToString() ?? responseData?.error?.ToString() ?? "Token not found in API response.";
                        ModelState.AddModelError("", $"Login failed: {errorMessage}");
                        Console.WriteLine($"Login: Token not found. Error: {errorMessage}");
                        return View(model);
                    }

                    HttpContext.Session.SetString("Token", token);
                    HttpContext.Session.SetString("Email", model.Email);

                    var tokenHandler = new JwtSecurityTokenHandler();
                    var jwtToken = tokenHandler.ReadJwtToken(token);
                    // Log tất cả claims để debug
                    Console.WriteLine("Login: Token claims: " + string.Join(", ", jwtToken.Claims.Select(c => $"{c.Type}: {c.Value}")));

                    // Kiểm tra roles với tên claim "role" và các biến thể
                    var roles = jwtToken.Claims
                        .Where(c => c.Type == ClaimTypes.Role || c.Type == "role" || c.Type == "roles" || c.Type == "Role")
                        .Select(c => c.Value)
                        .ToList();

                    if (!roles.Any())
                    {
                        string errorMessage = "No roles found in token. Available claims: " + string.Join(", ", jwtToken.Claims.Select(c => c.Type));
                        ModelState.AddModelError("", errorMessage);
                        Console.WriteLine($"Login: {errorMessage}");
                        return View(model);
                    }

                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Email),
                new Claim(ClaimTypes.NameIdentifier, jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub" || c.Type == "nameid")?.Value ?? model.Email)
            };

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
                    Console.WriteLine($"Login: Success for email={model.Email}, Roles={string.Join(", ", roles)}");
                    return RedirectToAction("Index", "Home");
                }
                catch (JsonException ex)
                {
                    ModelState.AddModelError("", $"Error parsing API response: {ex.Message}");
                    Console.WriteLine($"Login: JSON parse error: {ex.Message}");
                    return View(model);
                }
                catch (SecurityTokenException ex)
                {
                    ModelState.AddModelError("", $"Error decoding JWT token: {ex.Message}");
                    Console.WriteLine($"Login: JWT decode error: {ex.Message}");
                    return View(model);
                }
            }

            // Xử lý lỗi từ API
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Login: API error: StatusCode={response.StatusCode}, Response={errorContent}");
            try
            {
                var errorObj = JsonConvert.DeserializeObject<dynamic>(errorContent);
                var errorMessage = errorObj?.message?.ToString() ?? errorObj?.error?.ToString() ?? $"Login failed: HTTP {response.StatusCode}";
                ModelState.AddModelError("", errorMessage);
            }
            catch
            {
                ModelState.AddModelError("", $"Login failed: HTTP {response.StatusCode}");
            }
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