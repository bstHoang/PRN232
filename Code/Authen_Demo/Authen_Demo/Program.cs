using Authen_Demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add DB context
builder.Services.AddDbContext<AuthenDemoContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

// 2. Add Identity services
//builder.Services.AddIdentity<User, Role>()
//    .AddEntityFrameworkStores<AuthenDemoContext>()
//    .AddDefaultTokenProviders();

// 3. Configure Identity cookie
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Login/Index"; // Trang login nếu chưa đăng nhập
//    options.AccessDeniedPath = "/Login/AccessDenied"; // Trang từ chối truy cập
//});

// 4. Add MVC services
builder.Services.AddControllersWithViews();

// 5. Add session
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Hết hạn sau 30 phút không hoạt động
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// --- Middleware (thứ tự quan trọng) ---

// 1. Static files (CSS, JS, ảnh...)
app.UseStaticFiles();

// 2. Routing
app.UseRouting();

// 3. Authentication
app.UseAuthentication();

// 4. Authorization
app.UseAuthorization();

// 5. Session
app.UseSession();

// 6. Routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Index}/{id?}");

app.Run();
