using System;
using Moq;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Test.Modules.Employees.UseCases;

public class GetEmployeeByIdUseCaseImplTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepository = new();
    private readonly GetEmployeeByIdUseCaseImpl _sut;

    public GetEmployeeByIdUseCaseImplTests()
    {
        _sut = new GetEmployeeByIdUseCaseImpl(_employeeRepository.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEmployee_WhenEmployeeExists()
    {
        var employeeId = Guid.NewGuid();
        var employee = EmployeeTestData.CreateEmployee(id: employeeId);

        _employeeRepository
            .Setup(r => r.GetByIdAsync(employeeId))
            .ReturnsAsync(employee);

        var result = await _sut.GetByIdAsync(employeeId);

        Assert.Same(employee, result);
    }

    [Fact]
    public async Task GetByIdAsync_ThrowsArgumentException_WhenIdEmpty()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _sut.GetByIdAsync(Guid.Empty));
    }
}
