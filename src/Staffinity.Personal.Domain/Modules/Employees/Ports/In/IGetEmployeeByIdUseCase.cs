using System;
using System.Threading.Tasks;
using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Domain.Modules.Employees.Ports.In;

internal interface IGetEmployeeByIdUseCase
{
    Task<Employee?> GetByIdAsync(Guid employeeId);
}
