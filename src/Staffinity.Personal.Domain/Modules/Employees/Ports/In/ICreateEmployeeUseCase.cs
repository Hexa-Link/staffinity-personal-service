using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Domain.Modules.Employees.Ports.In
{
        public interface ICreateEmployeeUseCase
        {
            Task<Employee?> CreateAsync(Employee employee);
        }

}
