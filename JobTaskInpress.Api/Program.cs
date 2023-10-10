using System.Text.Json.Serialization;
using JobTaskInpress.Api;
using JobTaskInpress.db;
using Microsoft.EntityFrameworkCore;
using NewProject.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions
    .ReferenceHandler = ReferenceHandler.IgnoreCycles);;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInjection();

builder.Services.AddDbContext<MainDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddDbContext<DailyTaskContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DateSummaryDb"));
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

await DataInitialize.Initialize(app);

app.Run();