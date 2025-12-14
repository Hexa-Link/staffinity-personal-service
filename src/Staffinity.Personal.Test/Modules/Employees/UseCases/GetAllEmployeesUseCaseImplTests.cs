using Moq;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Test.Modules.Employees.UseCases;

public class GetAllEmployeesUseCaseImplTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepository = new();
    private readonly GetAllEmployeesUseCaseImpl _sut;

    public GetAllEmployeesUseCaseImplTests()
    {
        _sut = new GetAllEmployeesUseCaseImpl(_employeeRepository.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmployees_WhenRepositoryProvidesData()
    {
        var employee = EmployeeTestData.CreateEmployee();
        Employee[]? employees = new[] { employee };
        _employeeRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(employees);

        var result = await _sut.GetAllAsync();

        Assert.Single(result);
        Assert.Equal(employee, result[0]);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsEmptyArray_WhenRepositoryReturnsNull()
    {
        _employeeRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync((Employee[]?)null);

        var result = await _sut.GetAllAsync();

        Assert.NotNull(result);
        Assert.Empty(result);
    }
}
