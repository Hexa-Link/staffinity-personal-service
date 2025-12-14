using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Application.Modules.Employees.Dtos;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Application.Modules.Employees.Validators;
using Staffinity.Personal.Application.Modules.Notifications.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;
using Staffinity.Personal.Infrastructure.Persistence;
using Staffinity.Personal.Infrastructure.Persistence.Employees;
using Staffinity.Personal.Infrastructure.Persistence.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Get connection string to connect to DB
// TODO: Replace this for environment variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the DbContext
builder.Services.AddDbContext<PersonalDbContext>(options =>
    options.UseNpgsql(
        connectionString,
        b => b.MigrationsAssembly("Staffinity.Personal.Infrastructure")
    )
);

// Create the healthchecks
builder.Services.AddHealthChecks().AddDbContextCheck<PersonalDbContext>();

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

// Mapping Endpoint
app.MapControllers();
app.MapHealthChecks("/health");
app.MapControllers();

// Generate visual documentation
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Run the application
app.Run();

public partial class Program { }
