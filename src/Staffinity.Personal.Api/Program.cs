using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Npgsql;
using Staffinity.Personal.Application.Modules.AiIntelligence.Ports.Out;
using Staffinity.Personal.Application.Modules.AiIntelligence.Services;
using Staffinity.Personal.Application.Modules.AiIntelligence.UseCases;
using Staffinity.Personal.Application.Modules.Auth.UseCases;
using Staffinity.Personal.Application.Modules.Employees.Dtos;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Application.Modules.Employees.Validators;
using Staffinity.Personal.Application.Modules.Notifications.UseCases;
using Staffinity.Personal.Application.Modules.Vacations.UseCases;
using Staffinity.Personal.Domain.Modules.AiIntelligence.Ports.Out;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.In;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.Out;
using Staffinity.Personal.Domain.Modules.Auth.Ports.Out;
using Staffinity.Personal.Infrastructure.Adapters.Ai;
using Staffinity.Personal.Infrastructure.Adapters.Ai.ContextAdapters;
using Staffinity.Personal.Infrastructure.Persistence;
using Staffinity.Personal.Infrastructure.Persistence.Auth;
using Staffinity.Personal.Infrastructure.Persistence.Employees;
using Staffinity.Personal.Infrastructure.Persistence.Notifications;
using Staffinity.Personal.Infrastructure.Persistence.Vacations;
using Staffinity.Personal.Infrastructure.Security.Jwt;

var builder = WebApplication.CreateBuilder(args);

var databaseConnectionString = builder.Configuration.GetConnectionString("Default")
    ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(databaseConnectionString))
{
    throw new InvalidOperationException("ConnectionStrings:Default (or DefaultConnection) must be configured.");
}

if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<PersonalDbContext>(options =>
        options.UseNpgsql(
            databaseConnectionString,
            b => b.MigrationsAssembly("Staffinity.Personal.Infrastructure")
        )
    );

    builder.Services.AddHealthChecks().AddDbContextCheck<PersonalDbContext>();
}
else
{
    builder.Services.AddHealthChecks();
}

builder.Services.AddSingleton(_ => NpgsqlDataSource.Create(databaseConnectionString));

var enableHttpsRedirection = builder.Configuration["ASPNETCORE_URLS"]
    ?.Split(';', StringSplitOptions.RemoveEmptyEntries)
    .Any(url => url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) ?? false;

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSection);
var jwtSettings = jwtSection.Get<JwtSettings>();

if (jwtSettings is null)
{
    throw new InvalidOperationException("Jwt configuration section is required.");
}

if (string.IsNullOrWhiteSpace(jwtSettings.Issuer) ||
    string.IsNullOrWhiteSpace(jwtSettings.Audience) ||
    string.IsNullOrWhiteSpace(jwtSettings.Secret))
{
    throw new InvalidOperationException("Jwt:Issuer, Jwt:Audience and Jwt:Secret must be configured.");
}

var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = signingKey
        };
    });

builder.Services.AddAuthorization();

// Register dependencies
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IGetAllNotificationsUseCase, GetAllNotificationsUseCaseImpl>();
builder.Services.AddScoped<IGetNotificationByIdUseCase, GetNotificationByIdUseCaseImpl>();
builder.Services.AddScoped<ICreateNotificationUseCase, CreateNotificationUseCaseImpl>();
builder.Services.AddScoped<IUpdateNotificationUseCase, UpdateNotificationUseCaseImpl>();
builder.Services.AddScoped<IDeleteNotificationUseCase, DeleteNotificationUseCaseImpl>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IGetAllEmployeesUseCase, GetAllEmployeesUseCaseImpl>();
builder.Services.AddScoped<IGetEmployeeByIdUseCase, GetEmployeeByIdUseCaseImpl>();
builder.Services.AddScoped<ICreateEmployeeUseCase, CreateEmployeeUseCaseImpl>();
builder.Services.AddScoped<IUpdateEmployeeUseCase, UpdateEmployeeUseCaseImpl>();
builder.Services.AddScoped<IDeleteEmployeeUseCase, DeleteEmployeeUseCaseImpl>();
builder.Services.AddScoped<IValidator<CreateEmployeeRequest>, CreateEmployeeDtoValidator>();
builder.Services.AddScoped<IValidator<Employee>, UpdateEmployeeDtoValidator>();
builder.Services.AddScoped<IVacationRequestRepository, VacationRequestRepository>();
builder.Services.AddScoped<ICreateVacationRequestUseCase, CreateVacationRequestUseCase>();
builder.Services.AddScoped<IApproveVacationUseCase, ApproveVacationUseCase>();
builder.Services.AddScoped<IRejectVacationUseCase, RejectVacationUseCase>();
builder.Services.AddScoped<AskAiWithContextUseCase>();

builder.Services.AddScoped<IAuthRepository, PgAuthRepository>();
builder.Services.AddScoped<ILoginUseCase, LoginUseCase>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();


// AI Gemini Adapter
DotNetEnv.Env.Load(); // Load environment variables from .env file
builder.Services.AddSingleton(_ => GeminiOptions.FromEnvironment());

builder.Services.AddHttpClient<IAiModelClient, GeminiAiClient>(
    (sp, client) =>
    {
        var opt = sp.GetRequiredService<GeminiOptions>();
        client.BaseAddress = opt.BaseUri;
        client.Timeout = Timeout.InfiniteTimeSpan; // timeout lo control CTS into client
    }
);

// AI Services Registration
builder.Services.AddHttpClient<IIntentDetector, LlmIntentDetector>(
    (sp, client) =>
    {
        var opt = sp.GetRequiredService<GeminiOptions>();
        client.BaseAddress = opt.BaseUri;
        client.Timeout = TimeSpan.FromSeconds(10);
    }
);

builder.Services.AddScoped<IContextBuilder, ContextBuilder>();
builder.Services.AddSingleton<IStrategyRouter, StrategyRouter>();

// AI Context Adapters
builder.Services.AddScoped<IEmployeesAiContextPort, EmployeesAiContextAdapter>();
builder.Services.AddScoped<IVacationsAiContextPort, VacationsAiContextAdapter>();
builder.Services.AddScoped<INotificationsAiContextPort, NotificationsAiContextAdapter>();

// Add controllers
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    var bearerScheme = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    c.AddSecurityDefinition("Bearer", bearerScheme);
    c.AddSecurityRequirement(_ => new OpenApiSecurityRequirement
    {
        { new OpenApiSecuritySchemeReference("Bearer", null, null), new List<string>() }
    });
});

// Build app with all services
var app = builder.Build();

// Apply database migrations automatically on startup (like Flyway in Java)
if (!app.Environment.IsEnvironment("Testing"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<PersonalDbContext>();

    // Execute Custom Init Script (Flyway-style)
    try
    {
        var scriptPath = Path.Combine(AppContext.BaseDirectory, "Database", "init_schema.sql");
        if (File.Exists(scriptPath))
        {
            var sql = File.ReadAllText(scriptPath);
            db.Database.ExecuteSqlRaw(sql);
            Console.WriteLine("Successfully executed init_schema.sql");
        }
        else
        {
            Console.WriteLine($"Migration script not found at {scriptPath}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error executing migration script: {ex.Message}");
    }

    // Execute Seeder
    var seedPath = Path.Combine(AppContext.BaseDirectory, "Database", "seed_data.sql");
    if (File.Exists(seedPath))
    {
        var seedSql = File.ReadAllText(seedPath);
        db.Database.ExecuteSqlRaw(seedSql);
        Console.WriteLine("Successfully executed seed_data.sql");
    }
    else
    {
        Console.WriteLine($"Seeder script not found at {seedPath}");
    }

    // db.Database.Migrate(); // Commented out to avoid conflicts with manual SQL script. Uncomment if using EF Migrations.
}

// Generate visual documentation
app.UseSwagger();
app.UseSwaggerUI();

if (enableHttpsRedirection)
{
    app.UseHttpsRedirection();
}
app.UseAuthentication();
app.UseAuthorization();

// Mapping Endpoint
app.MapControllers();
app.MapHealthChecks("/health");

// Run the application
app.Run();

public partial class Program { }
