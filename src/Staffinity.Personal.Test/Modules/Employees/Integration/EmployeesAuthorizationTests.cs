using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Staffinity.Personal.Application.Modules.Employees.Dtos;
using Staffinity.Personal.Domain.Modules.Auth.Model;
using Staffinity.Personal.Test.Common;
using Staffinity.Personal.Test.Modules.Employees;
using Xunit;

namespace Staffinity.Personal.Test.Modules.Employees.Integration;

public class EmployeesAuthorizationTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public EmployeesAuthorizationTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAll_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/employees");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetAll_WithEmployeeRole_ReturnsOk()
    {
        var token = JwtTestTokenHelper.CreateToken(CustomWebApplicationFactory.TestJwtSecret, AccessLevelRoles.Employee);
        var request = new HttpRequestMessage(HttpMethod.Get, "/employees");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithEmployeeRole_ReturnsForbidden()
    {
        var token = JwtTestTokenHelper.CreateToken(CustomWebApplicationFactory.TestJwtSecret, AccessLevelRoles.Employee);
        var request = BuildCreateRequest(token);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task Create_WithAdminRole_ReturnsCreated()
    {
        var token = JwtTestTokenHelper.CreateToken(CustomWebApplicationFactory.TestJwtSecret, AccessLevelRoles.Admin);
        var request = BuildCreateRequest(token);

        var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }

    private static HttpRequestMessage BuildCreateRequest(string token)
    {
        var request = EmployeeTestData.CreateEmployeeRequest();
        var message = new HttpRequestMessage(HttpMethod.Post, "/employees")
        {
            Content = JsonContent.Create(request)
        };
        message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return message;
    }
}
