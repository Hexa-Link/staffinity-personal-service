using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Staffinity.Personal.Api.Modules.AiIntelligence.Dtos;
using Staffinity.Personal.Test.Common;
using Xunit;

namespace Staffinity.Personal.Test.Modules.AiIntelligence.Controller;

public sealed class AiControllerTests
    : IClassFixture<InMemoryWebApplicationFactory>
{
    private readonly HttpClient _client;

    public AiControllerTests(InMemoryWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Post_AiQuery_AsHr_ReturnsOkWithInsight()
    {
        // Arrange
        var request = new AiQueryRequestDto
        {
            Question = "Give me HR insights about employee engagement"
        };
        _client.DefaultRequestHeaders.Remove("X-Test-Role");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "HR");

        // Act
        var response = await _client.PostAsJsonAsync(
            "/ai/query",
            request
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content
            .ReadFromJsonAsync<AiQueryResponseDto>();

        body.Should().NotBeNull();
        body!.Summary.Should().NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Post_AiQuery_AsAdmin_ReturnsOkWithInsight()
    {
        // Arrange
        var request = new AiQueryRequestDto
        {
            Question = "Give me Admin insights"
        };
        _client.DefaultRequestHeaders.Remove("X-Test-Role");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Admin");

        // Act
        var response = await _client.PostAsJsonAsync(
            "/ai/query",
            request
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content
            .ReadFromJsonAsync<AiQueryResponseDto>();

        body.Should().NotBeNull();
    }

    [Fact]
    public async Task Post_AiQuery_InvalidRole_ReturnsForbidden()
    {
        // Arrange
        var request = new AiQueryRequestDto
        {
            Question = "I am an employee"
        };
        _client.DefaultRequestHeaders.Remove("X-Test-Role");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "Employee");

        // Act
        var response = await _client.PostAsJsonAsync(
            "/ai/query",
            request
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task Post_AiQuery_VerifyDtoMapping()
    {
        // Arrange
        var request = new AiQueryRequestDto
        {
            Question = "Test mapping"
        };
        _client.DefaultRequestHeaders.Remove("X-Test-Role");
        _client.DefaultRequestHeaders.Add("X-Test-Role", "HR");

        // Act
        var response = await _client.PostAsJsonAsync(
            "/ai/query",
            request
        );

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await response.Content
            .ReadFromJsonAsync<AiQueryResponseDto>();

        body.Should().NotBeNull();
        // Based on FakeAiModelClient implementation
        body!.Summary.Should().Be("This is a fake AI response for testing");
        body.Severity.Should().Be("Info");
        body.Recommendations.Should().BeEmpty();
    }
}