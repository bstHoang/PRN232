using Q2;

var builder = WebApplication.CreateBuilder(args);

//Initialize UrlUtilities with configuration
//DO NOT change this code
Utilities.Initialize(builder.Configuration);
//End

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Movies}/{action=Director_Movie}/{id?}"
    );
});

app.Run();
