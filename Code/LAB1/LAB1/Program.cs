using LAB1.Mapping;
using LAB1.Models;
using LAB1.Services;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// dang ki database
builder.Services.AddDbContext<NewsWebsiteDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

// dang ki auto mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

//dang ki DI
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<INewsService, NewsService>();

//dang ki odata
var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<Category>("Categories");
odataBuilder.EntitySet<News>("News");
builder.Services.AddControllers().AddOData(opt =>
    opt.AddRouteComponents("odata", odataBuilder.GetEdmModel()).Filter().Select().Expand().OrderBy().Count().SetMaxTop(100));


builder.Services.AddEndpointsApiExplorer();

// dang ki giao dien api
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewsWebsite API", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
