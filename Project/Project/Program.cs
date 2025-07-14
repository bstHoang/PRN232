// Program.cs
using AutoMapper;
using Microsoft.AspNetCore.Http; // Thêm cho Session
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project.Interfaces;
using Project.Mapper;
using Project.Models;
using Project.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Cấu hình Session
builder.Services.AddDistributedMemoryCache(); // Lưu Session trong bộ nhớ
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session hết hạn sau 30 phút
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Thêm IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<ProjectDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAutoMapper(typeof(UserProfile));
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

builder.Services.AddSwaggerGen(); 

var app = builder.Build();

app.UseCors("AllowAll");
app.UseSession(); // Kích hoạt Session
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();
app.Run();