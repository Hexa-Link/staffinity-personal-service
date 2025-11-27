using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.ValueObjects;

namespace Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

public interface IEmployeeRepository
{
    Task CreateAsync(Employee employee);

    Task<Employee?> GetByIdAsync(EmployeeId id);

    Task<IEnumerable<Employee>> GetAllAsync();
}
