using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Staffinity.Personal.Application.Modules.Vacations.Dto;
using Xunit;

namespace Staffinity.Personal.Tests.Integration
{
    // We use IClassFixture to create a test server based on your Program.cs
    public class VacationIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        // Constructor: This runs once before the tests in this class
        public VacationIntegrationTests(WebApplicationFactory<Program> factory)
        {
            // We create an HTTP Client to make requests to our API.
            // Since we are NOT replacing the database with InMemory,
            // this will connect to your REAL PostgreSQL database defined in appsettings.json.
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_CreateVacation_ReturnsCreated_And_SavesToDb()
        {
            // Arrange: Setup the data for the vacation request
            var request = new CreateVacationRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                // Using future dates to ensure valid logic (Start date > Today)
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(15),
                Reason = "Integration test saving to Real Postgres",
            };

            // Act: Send a POST request to the endpoint
            var response = await _client.PostAsJsonAsync("/vacation-requests", request);

            // Read the response body to display it if the test fails
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert: Check if the status code is 201 Created.
            // If it fails, the 'because' message will show the real error from the server.
            response
                .StatusCode.Should()
                .Be(HttpStatusCode.Created, because: $"Server returned error: {responseBody}");
        }
    }
}
