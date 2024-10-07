using Microsoft.EntityFrameworkCore;
using MinimalAPI.Services;
using MinimalApiProject;
using MinimalApiProject.Data;
using MinimalApiProject.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger middleware for API docs
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Global error handling middleware
app.UseExceptionHandler("/error");

// Map endpoints for Weather Forecast
app.MapWeatherForecastEndpoints();

app.Run();
