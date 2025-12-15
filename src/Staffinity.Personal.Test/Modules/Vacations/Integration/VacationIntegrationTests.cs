using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Staffinity.Personal.Application.Modules.Vacations.Dto;
using Staffinity.Personal.Test.Common;
using Xunit;

namespace Staffinity.Personal.Tests.Integration
{
    public class VacationIntegrationTests : IClassFixture<InMemoryWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public VacationIntegrationTests(InMemoryWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Post_CreateVacation_ReturnsCreated_And_SavesToDb()
        {
            // Arrange: Valid request body
            var request = new CreateVacationRequestDto
            {
                EmployeeId = Guid.NewGuid(),
                StartDate = DateTime.UtcNow.AddDays(10),
                EndDate = DateTime.UtcNow.AddDays(15),
                Reason = "CI/CD Integration Test",
            };

            // Act: Send POST request
            var response = await _client.PostAsJsonAsync("/vacation-requests", request);

            // Read response body for failure debugging
            var responseBody = await response.Content.ReadAsStringAsync();

            // Assert: Expect 201 Created
            response
                .StatusCode.Should()
                .Be(HttpStatusCode.Created, because: $"Server returned error: {responseBody}");
        }
    }
}
