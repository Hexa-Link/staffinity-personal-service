using Staffinity.Personal.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//load config database (PostgreSQL)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the DbContext (Necesario para que funcione el HealthCheck de abajo)
builder.Services.AddDbContext<PersonalDbContext>(options =>
    options.UseNpgsql(connectionString,
        b => b.MigrationsAssembly("Staffinity.Personal.Infrastructure")));

// Create the healthchecks
// (Esto configura el servicio ANTES de construir la app)
builder.Services.AddHealthChecks()
    .AddDbContextCheck<PersonalDbContext>();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Si usas Swagger clásico en lugar de OpenAPI nativo, descomenta esto:
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

// --- AQUÍ SE CONSTRUYE LA APLICACIÓN ---
var app = builder.Build(); 

// Mapping Endpoint in Docker
// (Esto se hace DESPUÉS de construir la app, usando la variable 'app')
app.MapHealthChecks("/health");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Ejemplo por defecto de .NET (WeatherForecast)
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}