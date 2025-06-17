using CompanyManage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Đăng ký DbContext
builder.Services.AddDbContext<CompanyDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

// Đăng ký Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<CompanyDbContext>()
.AddDefaultTokenProviders();

// Thêm ClaimsPrincipalFactory để thêm claims tùy chỉnh
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();

// Thêm Session
builder.Services.AddSession();

// Thêm MVC
builder.Services.AddControllersWithViews();

// Định nghĩa các chính sách phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HRManagerPolicy", policy =>
        policy.RequireRole("User")
              .RequireClaim("Department", "HR")
              .RequireClaim("Position", "Manager"));

    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("ViewEmployeeListPolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("User") &&
            (context.User.HasClaim("Department", "Board of Directors") && context.User.HasClaim("Position", "CEO") ||
             context.User.HasClaim("Department", "HR"))));

    options.AddPolicy("ViewProfilePolicy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("User") || context.User.IsInRole("Admin")));
});

var app = builder.Build();

// Cấu hình pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

// Khởi tạo người dùng mẫu với mật khẩu
using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var user = await userManager.FindByIdAsync("1");
    if (user != null && !await userManager.HasPasswordAsync(user))
    {
        await userManager.AddPasswordAsync(user, "Password123!");
    }
}

app.Run();