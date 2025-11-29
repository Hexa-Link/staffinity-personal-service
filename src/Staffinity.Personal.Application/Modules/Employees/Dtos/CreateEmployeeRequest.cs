namespace Staffinity.Personal.Application.Modules.Employees.Dtos;

public record CreateEmployeeRequest(
    string Code,
    string Name,
    string Email,
    string Password,
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
