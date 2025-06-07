using HW2.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Hw1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

builder.Services.AddControllers(options =>
{
    options.RespectBrowserAcceptHeader = true; 
})
.AddXmlSerializerFormatters() 
.AddMvcOptions(options =>
{
    options.OutputFormatters.Add(new CsvOutputFormatter()); 
});


var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();


app.Run();
