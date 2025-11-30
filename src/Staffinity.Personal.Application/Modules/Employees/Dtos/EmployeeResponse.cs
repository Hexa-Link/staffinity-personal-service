namespace Staffinity.Personal.Application.Modules.Employees.Dtos;

public record EmployeeResponse(
    Guid Id,
    string Code,
    string Name,
    string Email,
    string Phone,
    DateOnly BirthDate,
    DateOnly HireDate,
    Guid IdentificationTypeId,
    string IdentificationNumber,
    Guid? ManagerId,
    Guid HeadquartersId,
    Guid GenderId,
    Guid StatusId,
    Guid AccessLevelId);
