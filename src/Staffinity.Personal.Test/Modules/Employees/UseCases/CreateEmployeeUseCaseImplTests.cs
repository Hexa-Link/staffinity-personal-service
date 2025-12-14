using System;
using System.Threading;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Test.Modules.Employees.UseCases;

public class CreateEmployeeUseCaseImplTests
{
    private readonly Mock<IEmployeeRepository> _employeeRepository = new();
    private readonly Mock<IValidator<Employee>> _validator = new();
    private readonly CreateEmployeeUseCaseImpl _sut;

    public CreateEmployeeUseCaseImplTests()
    {
        _sut = new CreateEmployeeUseCaseImpl(_employeeRepository.Object, _validator.Object);
    }

    [Fact]
    public async Task CreateAsync_ReturnsEmployee_WhenDataIsValid()
    {
        var employee = EmployeeTestData.CreateEmployee();

        _validator
            .Setup(v => v.ValidateAsync(employee, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _employeeRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(Array.Empty<Employee>());
        _employeeRepository
            .Setup(r => r.CreateAsync(employee))
            .ReturnsAsync(employee);

        var result = await _sut.CreateAsync(employee);

        Assert.Same(employee, result);
        _employeeRepository.Verify(r => r.CreateAsync(employee), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ReturnsNull_WhenEmployeeIsNull()
    {
        var result = await _sut.CreateAsync(null!);

        Assert.Null(result);
        _validator.Verify(v => v.ValidateAsync(It.IsAny<Employee>(), It.IsAny<CancellationToken>()), Times.Never);
        _employeeRepository.Verify(r => r.GetAllAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ReturnsNull_WhenValidationFails()
    {
        var employee = EmployeeTestData.CreateEmployee();
        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Email", "Invalid format")
        });

        _validator
            .Setup(v => v.ValidateAsync(employee, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        var result = await _sut.CreateAsync(employee);

        Assert.Null(result);
        _employeeRepository.Verify(r => r.CreateAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ReturnsNull_WhenEmailIsDuplicated()
    {
        var employee = EmployeeTestData.CreateEmployee(email: "duplicate@example.com");
        var other = EmployeeTestData.CreateEmployee(email: employee.Email);

        _validator
            .Setup(v => v.ValidateAsync(employee, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _employeeRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(new[] { other });

        var result = await _sut.CreateAsync(employee);

        Assert.Null(result);
        _employeeRepository.Verify(r => r.CreateAsync(It.IsAny<Employee>()), Times.Never);
    }

    [Fact]
    public async Task CreateAsync_ReturnsNull_WhenRepositoryReturnsNull()
    {
        var employee = EmployeeTestData.CreateEmployee();

        _validator
            .Setup(v => v.ValidateAsync(employee, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _employeeRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(Array.Empty<Employee>());
        _employeeRepository
            .Setup(r => r.CreateAsync(employee))
            .ReturnsAsync((Employee?)null);

        var result = await _sut.CreateAsync(employee);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ReturnsNull_WhenRepositoryThrows()
    {
        var employee = EmployeeTestData.CreateEmployee();

        _validator
            .Setup(v => v.ValidateAsync(employee, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _employeeRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(Array.Empty<Employee>());
        _employeeRepository
            .Setup(r => r.CreateAsync(employee))
            .ThrowsAsync(new InvalidOperationException("failed"));

        var result = await _sut.CreateAsync(employee);

        Assert.Null(result);
    }
}
