namespace Staffinity.Personal.Application.Modules.Employees.Dtos;

public record EmployeeResponse(
    Guid Id,
    string Code,
    string FirstName,
    string? MiddleName,
    string LastName,
    string? SecondLastName,
    string Email,
    string? PhoneNumber,
    DateTime DateOfBirth,
    DateTime HireDate,
    Guid IdentificationTypeId,
    string IdentificationNumber,
    Guid? ManagerId,
    Guid HeadquartersId,
    Guid GenderId,
    Guid StatusId,
    Guid AccessLevelId);
