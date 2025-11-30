using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;
using Staffinity.Personal.Infrastructure.Persistence;
using Staffinity.Personal.Infrastructure.Persistence.Employees;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PersonalDbContext>(options =>
    options.UseNpgsql(connectionString,
        b => b.MigrationsAssembly("Staffinity.Personal.Infrastructure")));

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<CreateEmployeeUseCase>();
builder.Services.AddScoped<GetEmployeesUseCase>();
builder.Services.AddScoped<GetEmployeeByIdUseCase>();
builder.Services.AddScoped<UpdateEmployeeUseCase>();
builder.Services.AddScoped<DeleteEmployeeUseCase>();

builder.Services.AddHealthChecks()
    .AddDbContextCheck<PersonalDbContext>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();
// builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapHealthChecks("/health");
app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // app.UseSwagger();
    // app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
