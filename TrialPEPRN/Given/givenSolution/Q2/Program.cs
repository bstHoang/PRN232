var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    // Đặt route mặc định là controller Movies, action Director_Movie
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Movies}/{action=Director_Movie}/{id?}"
    );
});

app.Run();
