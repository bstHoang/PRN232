using AdminClient.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddSession(); // Thêm session để lưu JWT token
builder.Services.AddHttpClient(); // Để gọi API
builder.Services.AddHttpContextAccessor(); // Để truy cập HttpContext trong ApiService
builder.Services.AddScoped<ApiService>(); // Đăng ký ApiService

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Sử dụng session middleware

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();