using CompanyManage.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// Thêm ClaimsPrincipalFactory
builder.Services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, CustomUserClaimsPrincipalFactory>();

// Thêm Session
builder.Services.AddSession();

// Configure JWT authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
    // Trả lỗi 401 thay vì chuyển hướng
    options.Events = new JwtBearerEvents
    {
        OnChallenge = context =>
        {
            context.HandleResponse();
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("{\"error\": \"Unauthorized\"}");
        }
    };
});

// Thêm MVC và API
builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

// Định nghĩa chính sách phân quyền
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("HRManagerPolicy", policy =>
        policy.RequireRole("User")
              .RequireClaim("Department", "HR")
              .RequireClaim("Position", "Manager"));

    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireRole("Admin"));

    options.AddPolicy("ViewEmployeeListPolicy", policy =>
        policy.RequireRole("User")
              .RequireClaim("Department", "HR"));

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
app.MapControllers();

app.Run();