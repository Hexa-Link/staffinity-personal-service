using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Application.Modules.Notifications.UseCases;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;
using Staffinity.Personal.Infrastructure.Persistence;
using Staffinity.Personal.Infrastructure.Persistence.Notifications;

var builder = WebApplication.CreateBuilder(args);

// Get connection string to connect to DB
// TODO: Replace this for environment variables
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the DbContext
builder.Services.AddDbContext<PersonalDbContext>(options =>
    options.UseNpgsql(connectionString,
        b => b.MigrationsAssembly("Staffinity.Personal.Infrastructure")));

// Create the healthchecks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<PersonalDbContext>();

// Register dependencies
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IGetAllNotificationsUseCase, GetAllNotificationsUseCaseImpl>();
builder.Services.AddScoped<IGetNotificationByIdUseCase, GetNotificationByIdUseCaseImpl>();

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

// Generate visual documentation
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Run the application
app.Run();