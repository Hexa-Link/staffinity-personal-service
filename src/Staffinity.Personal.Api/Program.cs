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
using Staffinity.Personal.Application.Modules.AiIntelligence.Ports.Out;
using Staffinity.Personal.Application.Modules.AiIntelligence.Services;
using Staffinity.Personal.Infrastructure.Adapters.Ai;
using Staffinity.Personal.Infrastructure.Adapters.Ai.ContextAdapters;
using Staffinity.Personal.Infrastructure.Persistence;
using Staffinity.Personal.Infrastructure.Persistence.Employees;
using Staffinity.Personal.Infrastructure.Persistence.Notifications;
using Staffinity.Personal.Infrastructure.Persistence.Vacations;

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

// Mapping Endpoint
app.MapControllers();
app.MapHealthChecks("/health");

// Generate visual documentation
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Run the application
app.Run();

public partial class Program { }
