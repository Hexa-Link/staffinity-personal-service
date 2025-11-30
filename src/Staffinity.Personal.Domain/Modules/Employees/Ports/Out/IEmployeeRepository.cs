using System;
using System.Threading.Tasks;
using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

public interface IEmployeeRepository
{
    Task<Employee[]> GetAllAsync();
    Task<Employee?> GetByIdAsync(Guid employeeId);
    Task<Employee?> CreateAsync(Employee employee);
    Task<Employee?> UpdateAsync(Employee employee);
    Task<bool> DeleteAsync(Guid employeeId);
}

