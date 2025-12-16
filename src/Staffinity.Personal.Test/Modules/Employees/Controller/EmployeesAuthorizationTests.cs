using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Staffinity.Personal.Domain.Modules.Auth.Model;
using Staffinity.Personal.Test.Common;
using Xunit;

namespace Staffinity.Personal.Test.Modules.Employees.Controller;

public sealed class EmployeesAuthorizationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly JwtTestTokenHelper _tokenHelper;

    public EmployeesAuthorizationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _tokenHelper = new JwtTestTokenHelper(
            CustomWebApplicationFactory.JwtSecret,
            CustomWebApplicationFactory.JwtIssuer,
            CustomWebApplicationFactory.JwtAudience);
    }

    [Fact]
    public async Task GetEmployees_WithoutToken_ReturnsUnauthorized()
    {
        var client = _factory.CreateClient();

        var response = await client.GetAsync("/employees");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetEmployees_WithEmployeeRole_ReturnsOk()
    {
        var client = _factory.CreateClient();
        Authenticate(client, AccessLevelRoles.Employee);

        var response = await client.GetAsync("/employees");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task CreateEmployee_WithEmployeeRole_ReturnsForbidden()
    {
        var client = _factory.CreateClient();
        Authenticate(client, AccessLevelRoles.Employee);

        var response = await client.PostAsync("/employees", BuildCreatePayload());

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CreateEmployee_WithAdminRole_ReturnsCreated()
    {
        var client = _factory.CreateClient();
        Authenticate(client, AccessLevelRoles.Admin);

        var response = await client.PostAsync("/employees", BuildCreatePayload());

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    private void Authenticate(HttpClient client, string role)
    {
        client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", _tokenHelper.CreateToken(role));
    }

    private static HttpContent BuildCreatePayload()
    {
        var request = EmployeeTestData.CreateEmployeeRequest();
        var serialized = JsonSerializer.Serialize(request);
        return new StringContent(serialized, Encoding.UTF8, "application/json");
    }
}
