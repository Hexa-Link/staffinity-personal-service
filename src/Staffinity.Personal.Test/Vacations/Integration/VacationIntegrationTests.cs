using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Staffinity.Personal.Api; // Asegúrate de que referencie a tu API
using Staffinity.Personal.Application.Modules.Vacations.DTOs;
using Staffinity.Personal.Infrastructure.Persistence; // Donde esté tu DbContext
using Xunit;

namespace Staffinity.Personal.Tests.Integration
{
    // WebApplicationFactory levanta la API en memoria
    public class VacationIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public VacationIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // Aquí configuramos la API para usar Base de Datos en Memoria
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // 1. Quitamos la configuración de DB real (SQL Server/Postgres)
                    services.RemoveAll(typeof(DbContextOptions<PersonalDbContext>));

                    // 2. Agregamos la DB en Memoria para testing
                    services.AddDbContext<PersonalDbContext>(options =>
                    {
                        options.UseInMemoryDatabase("TestDb_Vacations");
                    });
                });
            });
        }

        [Fact]
        public async Task Post_CreateVacation_ReturnsCreated_And_SavesToDb()
        {
            // Arrange: Creamos el cliente HTTP (como si fuera Postman)
            var client = _factory.CreateClient();

            var newRequest = new CreateVacationRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(20),
                Reason = "Integration Test Vacation",
            };

            // Act: Hacemos el POST real a la ruta
            var response = await client.PostAsJsonAsync("/vacation-requests", newRequest);

            // Assert: Verificamos que responda 201 Created
            response.StatusCode.Should().Be(HttpStatusCode.Created); // Si usas FluentAssertions
            // O: Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Post_CreateVacation_ReturnsBadRequest_WhenDatesAreInvalid()
        {
            // Arrange
            var client = _factory.CreateClient();

            var invalidRequest = new CreateVacationRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow.AddDays(-5), // Fecha en el pasado (ERROR)
                EndDate = DateTime.UtcNow.AddDays(5),
                Reason = "Invalid Dates",
            };

            // Act
            var response = await client.PostAsJsonAsync("/vacation-requests", invalidRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
