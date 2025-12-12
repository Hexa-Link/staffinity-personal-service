using Staffinity.Personal.Application.Modules.Employees.Dtos;
using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Test.Modules.Employees;

internal static class EmployeeTestData
{
    public const string DefaultPassword = "Pa$$w0rd";
    public const string DefaultPasswordHash = "zqk3QJfYGF9vd85EYkE0MccYnxeDPwMW6xgob5r0GTQ=";

    public static Employee CreateEmployee(
        Guid? id = null,
        string? email = null,
        string? code = null)
    {
        var now = DateTime.UtcNow;
        var employeeId = id ?? Guid.NewGuid();
        var generatedEmail = email ?? $"employee{employeeId:N}@example.com";
        return new Employee
        {
            Id = employeeId,
            Code = code ?? $"CODE-{employeeId:N}",
            FirstName = "Jane",
            MiddleName = "Test",
            LastName = "Doe",
            SecondLastName = "Smith",
            Email = generatedEmail,
            PasswordHash = DefaultPasswordHash,
            PhoneNumber = "555-1234",
            DateOfBirth = now.AddYears(-25),
            HireDate = now.AddYears(-2),
            IdentificationTypeId = Guid.NewGuid(),
            IdentificationNumber = "ABC-12345",
            ManagerId = Guid.NewGuid(),
            HeadquartersId = Guid.NewGuid(),
            GenderId = Guid.NewGuid(),
            StatusId = Guid.NewGuid(),
            AccessLevelId = Guid.NewGuid(),
            CreatedAt = now,
            UpdatedAt = now,
            IsDeleted = false
        };
    }

    public static CreateEmployeeRequest CreateEmployeeRequest(string? password = DefaultPassword)
    {
        var now = DateTime.UtcNow;
        return new CreateEmployeeRequest
        {
            Code = $"C-{Guid.NewGuid():N}",
            FirstName = "Test",
            LastName = "User",
            Email = $"test{Guid.NewGuid():N}@example.com",
            Password = password ?? string.Empty,
            PhoneNumber = "555-0000",
            DateOfBirth = now.AddYears(-30),
            HireDate = now.AddYears(-1),
            IdentificationTypeId = Guid.NewGuid(),
            IdentificationNumber = "ID-67890",
            HeadquartersId = Guid.NewGuid(),
            GenderId = Guid.NewGuid(),
            StatusId = Guid.NewGuid(),
            AccessLevelId = Guid.NewGuid(),
            MiddleName = "Sample",
            SecondLastName = "Tester",
            ManagerId = Guid.NewGuid()
        };
    }

    public static UpdateEmployeeRequest CreateUpdateEmployeeRequest()
    {
        return new UpdateEmployeeRequest
        {
            Code = "UPDATED-CODE",
            FirstName = "Updated",
            MiddleName = "Middle",
            LastName = "User",
            SecondLastName = "Updated",
            Email = "updated@example.com",
            PhoneNumber = "555-9999",
            DateOfBirth = DateTime.UtcNow.AddYears(-28),
            HireDate = DateTime.UtcNow.AddYears(-2),
            IdentificationTypeId = Guid.NewGuid(),
            IdentificationNumber = "UPDATED-ID",
            ManagerId = Guid.NewGuid(),
            HeadquartersId = Guid.NewGuid(),
            GenderId = Guid.NewGuid(),
            StatusId = Guid.NewGuid(),
            AccessLevelId = Guid.NewGuid()
        };
    }
}
