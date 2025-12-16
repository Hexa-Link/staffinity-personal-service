using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using Npgsql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Application.Modules.AiIntelligence.UseCases;
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
using Staffinity.Personal.Infrastructure.Persistence;
using Staffinity.Personal.Infrastructure.Persistence.Employees;
using Staffinity.Personal.Infrastructure.Persistence.Notifications;
using Staffinity.Personal.Infrastructure.Persistence.Vacations;
using Staffinity.Personal.Infrastructure.Persistence.Auth;
using Staffinity.Personal.Application.Modules.Auth.UseCases;
using Staffinity.Personal.Infrastructure.Security.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Only configure PostgresSQL if not in testing mode
// In testing, the WebApplicationFactory will override with InMemory
if (!builder.Environment.IsEnvironment("Testing"))
{
    // Get connection string to connect to DB
    // TODO: Replace this for environment variables
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    // Register the DbContext with PostgresSQL
    builder.Services.AddDbContext<PersonalDbContext>(options =>
        options.UseNpgsql(
            connectionString,
            b => b.MigrationsAssembly("Staffinity.Personal.Infrastructure")
        )
    );

    // Create the health checks
    builder.Services.AddHealthChecks().AddDbContextCheck<PersonalDbContext>();
}
else
{
    // In testing mode, a placeholder DbContext will be registered by WebApplicationFactory
    // Register a basic health check service
    builder.Services.AddHealthChecks();
}

// Register dependencies
builder.Services.AddSingleton(sp =>
{
    var defaultConnection = builder.Configuration.GetConnectionString("Default");
    if (string.IsNullOrWhiteSpace(defaultConnection))
    {
        throw new InvalidOperationException("ConnectionStrings:Default must be configured.");
    }

    return NpgsqlDataSource.Create(defaultConnection);
});

builder.Services.AddScoped<IAuthRepository, PgAuthRepository>();
builder.Services.AddScoped<ILoginUseCase, LoginUseCase>();

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

var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtSettings>(jwtSection);

var jwtSettings = jwtSection.Get<JwtSettings>();
if (jwtSettings is null)
{
    throw new InvalidOperationException("Jwt configuration section is required.");
}

if (string.IsNullOrWhiteSpace(jwtSettings.Secret))
{
    throw new InvalidOperationException("Jwt:Secret must be configured via appsettings or environment.");
}

if (string.IsNullOrWhiteSpace(jwtSettings.Issuer) || string.IsNullOrWhiteSpace(jwtSettings.Audience))
{
    throw new InvalidOperationException("Jwt:Issuer and Jwt:Audience must be configured via appsettings or environment.");
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
            IssuerSigningKey = signingKey,
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// Add controllers
builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

    // db.Database.Migrate(); // Commented out to avoid conflicts with manual SQL script. Uncomment if using EF Migrations.
}

// Generate visual documentation
app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/swagger/v1/swagger.json", StringComparison.OrdinalIgnoreCase))
    {
        var originalBody = context.Response.Body;
        await using var buffer = new MemoryStream();
        context.Response.Body = buffer;

        await next();

        buffer.Seek(0, SeekOrigin.Begin);
        var swaggerJson = await new StreamReader(buffer).ReadToEndAsync();
        var enhancedJson = AddSwaggerSecurityDefinitions(swaggerJson);

        context.Response.Body = originalBody;
        context.Response.ContentType = "application/json";
        var encoded = Encoding.UTF8.GetBytes(enhancedJson);
        context.Response.ContentLength = encoded.Length;
        await context.Response.Body.WriteAsync(encoded);
        return;
    }

    await next();
});

app.UseSwagger();
app.UseSwaggerUI();

var httpsEnabled = (app.Configuration["ASPNETCORE_URLS"]?
        .Split(';', StringSplitOptions.RemoveEmptyEntries)
        .Any(url => url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) ?? false)
    || app.Urls.Any(url => url.StartsWith("https://", StringComparison.OrdinalIgnoreCase));

if (httpsEnabled)
{
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

// Run the application
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

static string AddSwaggerSecurityDefinitions(string swaggerJson)
{
    var root = JsonNode.Parse(swaggerJson) as JsonObject ?? new JsonObject();
    var components = root["components"] as JsonObject ?? new JsonObject();
    root["components"] = components;

    var securitySchemes = components["securitySchemes"] as JsonObject ?? new JsonObject();
    components["securitySchemes"] = securitySchemes;

    securitySchemes["Bearer"] = new JsonObject
    {
        ["type"] = "http",
        ["scheme"] = "bearer",
        ["bearerFormat"] = "JWT",
        ["description"] = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\""
    };

    root["security"] = new JsonArray
    {
        new JsonObject { ["Bearer"] = new JsonArray() }
    };

    return root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
}

public partial class Program { }
