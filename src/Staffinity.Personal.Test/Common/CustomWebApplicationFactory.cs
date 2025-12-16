using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Infrastructure.Persistence;
using Staffinity.Personal.Test.Modules.Employees;

namespace Staffinity.Personal.Test.Common;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string JwtIssuer = "staffinity-personal-test";
    public const string JwtAudience = "staffinity-personal-test";
    public const string JwtSecret = "staffinity-test-secret-1234567890";
    private const string TestConnectionString = "Host=localhost;Port=5432;Database=staffinity_personal_test;Username=postgres;Password=password";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("JWT__ISSUER", JwtIssuer);
        Environment.SetEnvironmentVariable("JWT__AUDIENCE", JwtAudience);
        Environment.SetEnvironmentVariable("JWT__SECRET", JwtSecret);
        Environment.SetEnvironmentVariable("JWT__EXPIRESMINUTES", "60");
        Environment.SetEnvironmentVariable("ConnectionStrings__Default", TestConnectionString);
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", TestConnectionString);

        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, configBuilder) =>
        {
            var overrides = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = TestConnectionString,
                ["Jwt:Issuer"] = JwtIssuer,
                ["Jwt:Audience"] = JwtAudience,
                ["Jwt:Secret"] = JwtSecret,
                ["Jwt:ExpiresMinutes"] = "60"
            };

            configBuilder.AddInMemoryCollection(overrides);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<IGetAllEmployeesUseCase>();
            services.AddScoped<IGetAllEmployeesUseCase, FakeGetAllEmployeesUseCase>();

            services.RemoveAll<ICreateEmployeeUseCase>();
            services.AddScoped<ICreateEmployeeUseCase, FakeCreateEmployeeUseCase>();

            services.AddDbContext<PersonalDbContext>(options =>
            {
                options.UseInMemoryDatabase("TestEmployeesAuthorizationDb");
            });
        });
    }

    private sealed class FakeGetAllEmployeesUseCase : IGetAllEmployeesUseCase
    {
        public Task<Employee[]> GetAllAsync()
        {
            var employees = new[] { EmployeeTestData.CreateEmployee() };
            return Task.FromResult(employees);
        }
    }

    private sealed class FakeCreateEmployeeUseCase : ICreateEmployeeUseCase
    {
        public Task<Employee?> CreateAsync(Employee employee)
        {
            var stub = EmployeeTestData.CreateEmployee(email: employee.Email, code: employee.Code);
            return Task.FromResult<Employee?>(stub);
        }
    }
}
