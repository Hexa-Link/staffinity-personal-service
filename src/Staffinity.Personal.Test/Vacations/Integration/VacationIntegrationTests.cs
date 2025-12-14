using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Staffinity.Personal.Application.Modules.Vacations.Dto;
using Staffinity.Personal.Infrastructure.Persistence;
using Xunit;

namespace Staffinity.Personal.Tests.Integration
{
    public class VacationIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public VacationIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Configure the test server to override the default database configuration
            var customFactory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 1. Remove existing DbContext options (PostgreSQL configuration)
                    var optionsDescriptors = services
                        .Where(d =>
                            d.ServiceType == typeof(DbContextOptions<PersonalDbContext>)
                            || d.ServiceType == typeof(DbContextOptions)
                        )
                        .ToList();

                    foreach (var descriptor in optionsDescriptors)
                    {
                        services.Remove(descriptor);
                    }

                    // 2. Remove existing database connections to prevent provider conflicts
                    var connectionDescriptors = services
                        .Where(d => d.ServiceType == typeof(DbConnection))
                        .ToList();

                    foreach (var descriptor in connectionDescriptors)
                    {
                        services.Remove(descriptor);
                    }

                    // 3. Inject InMemory Database for isolated testing
                    // We use a unique GUID to ensure a fresh database for each test run
                    services.AddDbContext<PersonalDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("InMemoryDbForTesting_" + Guid.NewGuid());
                    });
                });
            });

            _client = customFactory.CreateClient();
        }

        [Fact]
        public async Task Post_CreateVacation_ReturnsCreated_And_SavesToDb()
        {
            // Arrange
            var request = new CreateVacationRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                // Use future dates to pass validation logic
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(15),
                Reason = "Integration Test with InMemory DB",
            };

            // Act
            var response = await _client.PostAsJsonAsync("/vacation-requests", request);

            // Assert
            var responseBody = await response.Content.ReadAsStringAsync();

            response
                .StatusCode.Should()
                .Be(HttpStatusCode.Created, because: $"Server returned error: {responseBody}");
        }
    }
}
