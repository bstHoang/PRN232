using _10API.Models;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<Api10TestContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));

builder.Services.AddControllers()
    .AddXmlSerializerFormatters()
    .AddOData(opt =>
        opt.Select().Filter().OrderBy().Expand().SetMaxTop(100)
            .AddRouteComponents("odata", new ODataConventionModelBuilder().GetEdmModel()));

var app = builder.Build();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();

app.Run();
