using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Application.Modules.Employees.Dtos;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Application.Modules.Employees.Validators;
using Staffinity.Personal.Application.Modules.Notifications.UseCases;
using Staffinity.Personal.Application.Modules.Vacations.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.In;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.Out;
using Staffinity.Personal.Infrastructure.Persistence;
using Staffinity.Personal.Infrastructure.Persistence.Employees;
using Staffinity.Personal.Infrastructure.Persistence.Notifications;
using Staffinity.Personal.Infrastructure.Persistence.Vacations;

var builder = WebApplication.CreateBuilder(args);

// Only configure PostgreSQL if not in testing mode
// In testing, the WebApplicationFactory will override with InMemory
if (!builder.Environment.IsEnvironment("Testing"))
{
    // Get connection string to connect to DB
    // TODO: Replace this for environment variables
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    // Register the DbContext with PostgreSQL
    builder.Services.AddDbContext<PersonalDbContext>(options =>
        options.UseNpgsql(
            connectionString,
            b => b.MigrationsAssembly("Staffinity.Personal.Infrastructure")
        )
    );

    // Create the healthchecks
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
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<PersonalDbContext>();
        db.Database.Migrate();
    }
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
