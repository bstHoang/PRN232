using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Q1.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PePrnFall22B1Context>(options =>
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