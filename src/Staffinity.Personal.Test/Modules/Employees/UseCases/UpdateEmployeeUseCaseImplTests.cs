using System;
using System.Threading;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Test.Modules.Employees.UseCases;

public class UpdateEmployeeUseCaseImplTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepository = new();
    private readonly Mock<IValidator<Employee>> _validator = new();
    private readonly UpdateEmployeeUseCaseImpl _sut;

    public UpdateEmployeeUseCaseImplTests()
    {
        _sut = new UpdateEmployeeUseCaseImpl(_employeeRepository.Object, _validator.Object);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsEmployee_WhenDataIsValid()
    {
        var employee = EmployeeTestData.CreateEmployee();
        SetupValidValidator();
        _employeeRepository
            .Setup(r => r.GetByIdAsync(employee.Id))
            .ReturnsAsync(employee);
        _employeeRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new[] { employee });
        _employeeRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Employee>()))
            .ReturnsAsync((Employee e) => e);

        var result = await _sut.UpdateAsync(employee);

        Assert.NotNull(result);
        Assert.Equal(employee.Id, result!.Id);
        _employeeRepository.Verify(r => r.UpdateAsync(It.IsAny<Employee>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenEmployeeIsNull()
    {
        var result = await _sut.UpdateAsync(null!);

        Assert.Null(result);
        _validator.Verify(v => v.ValidateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenValidationFails()
    {
        var employee = EmployeeTestData.CreateEmployee();
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Email", "Invalid")
        });

        _validator
            .Setup(v => v.ValidateAsync(employee, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _sut.UpdateAsync(employee);

        Assert.Null(result);
        _employeeRepository.Verify(r => r.GetByIdAsync(It.IsAny<Guid>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenEmployeeNotFound()
    {
        var employee = EmployeeTestData.CreateEmployee();
        SetupValidValidator();

        _employeeRepository
            .Setup(r => r.GetByIdAsync(employee.Id))
            .ReturnsAsync((Employee?)null);

        var result = await _sut.UpdateAsync(employee);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenEmailAlreadyUsed()
    {
        var employee = EmployeeTestData.CreateEmployee(email: "duplicated@example.com");
        var duplicate = EmployeeTestData.CreateEmployee(email: employee.Email);
        SetupValidValidator();

        _employeeRepository
            .Setup(r => r.GetByIdAsync(employee.Id))
            .ReturnsAsync(employee);
        _employeeRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new[] { employee, duplicate });

        var result = await _sut.UpdateAsync(employee);

        Assert.Null(result);
        _employeeRepository.Verify(r => r.UpdateAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenRepositoryReturnsNull()
    {
        var employee = EmployeeTestData.CreateEmployee();
        SetupValidValidator();

        _employeeRepository
            .Setup(r => r.GetByIdAsync(employee.Id))
            .ReturnsAsync(employee);
        _employeeRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new[] { employee });
        _employeeRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Employee>()))
            .ReturnsAsync((Employee?)null);

        var result = await _sut.UpdateAsync(employee);

        Assert.Null(result);
    }

    [Fact]
    public async Task UpdateAsync_ReturnsNull_WhenRepositoryThrows()
    {
        var employee = EmployeeTestData.CreateEmployee();
        SetupValidValidator();

        _employeeRepository
            .Setup(r => r.GetByIdAsync(employee.Id))
            .ReturnsAsync(employee);
        _employeeRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new[] { employee });
        _employeeRepository
            .Setup(r => r.UpdateAsync(It.IsAny<Employee>()))
            .ThrowsAsync(new InvalidOperationException("failure"));

        var result = await _sut.UpdateAsync(employee);

        Assert.Null(result);
    }

    private void SetupValidValidator()
    {
        _validator
            .Setup(v => v.ValidateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
    }
}
