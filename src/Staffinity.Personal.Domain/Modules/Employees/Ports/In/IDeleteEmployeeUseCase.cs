using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Domain.Modules.Employees.Ports.In;
    internal interface IDeleteEmployeeUseCase
    {
        Task<bool> DeleteAsync(Employee employee);
    }

