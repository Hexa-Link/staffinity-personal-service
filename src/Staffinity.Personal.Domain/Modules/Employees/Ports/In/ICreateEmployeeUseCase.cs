using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.ValueObjects;

namespace Staffinity.Personal.Domain.Modules.Employees.Ports.In;

public interface ICreateEmployeeUseCase
{
    Task<Employee> ExecuteAsync(
        EmployeeId id,
        EmployeeCode code,
        PersonName name,
        EmployeeEmail email,
        string passwordHash,
        PhoneNumber phone,
        DateOnly birthDate,
        DateOnly hireDate,
        Guid identificationTypeId,
        string identificationNumber,
        EmployeeId? managerId,
        Guid headquartersId,
        Guid genderId,
        Guid statusId,
        Guid accessLevelId);
}
