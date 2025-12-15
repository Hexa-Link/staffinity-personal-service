using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Test.Modules.Employees;

namespace Staffinity.Personal.Test.Common;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public const string TestJwtSecret = "TestJwtSecretForEmployees123456789012";

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        Environment.SetEnvironmentVariable("Jwt__Issuer", JwtTestTokenHelper.Issuer);
        Environment.SetEnvironmentVariable("Jwt__Audience", JwtTestTokenHelper.Audience);
        Environment.SetEnvironmentVariable("Jwt__Secret", TestJwtSecret);
        Environment.SetEnvironmentVariable("ConnectionStrings__Default", "Host=localhost;Port=5432;Database=staffinity_personal_test;Username=test;Password=test;");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var settings = new Dictionary<string, string?>
            {
                ["ConnectionStrings:Default"] = "Host=localhost;Port=5432;Database=staffinity_personal_test;Username=test;Password=test;",
                ["Jwt:Issuer"] = "Staffinity.Personal.Api",
                ["Jwt:Audience"] = "Staffinity.Personal.Client",
                ["Jwt:Secret"] = TestJwtSecret,
                ["Jwt:ExpiresMinutes"] = "60"
            };

            config.AddInMemoryCollection(settings);
        });

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<ICreateEmployeeUseCase>();
            services.RemoveAll<IGetAllEmployeesUseCase>();
            services.RemoveAll<IEmployeeRepository>();

            services.AddScoped<ICreateEmployeeUseCase>(_ => new FakeCreateEmployeeUseCase());
            services.AddScoped<IGetAllEmployeesUseCase>(_ => new FakeGetAllEmployeesUseCase());
            services.AddScoped<IEmployeeRepository>(_ => new FakeEmployeeRepository());
        });

        base.ConfigureWebHost(builder);
    }

    private sealed class FakeCreateEmployeeUseCase : ICreateEmployeeUseCase
    {
        public Task<Employee?> CreateAsync(Employee employee) => Task.FromResult<Employee?>(employee);
    }

    private sealed class FakeGetAllEmployeesUseCase : IGetAllEmployeesUseCase
    {
        public Task<Employee[]> GetAllAsync() => Task.FromResult(new[] { EmployeeTestData.CreateEmployee() });
    }

    private sealed class FakeEmployeeRepository : IEmployeeRepository
    {
        public Task<Employee[]> GetAllAsync() => Task.FromResult(Array.Empty<Employee>());

        public Task<Employee?> GetByIdAsync(Guid id) => Task.FromResult<Employee?>(null);

        public Task<Employee?> CreateAsync(Employee employee) => Task.FromResult(employee);

        public Task<Employee?> UpdateAsync(Employee employee) => Task.FromResult(employee);

        public Task<bool> DeleteAsync(Guid employeeId) => Task.FromResult(true);
    }
}
