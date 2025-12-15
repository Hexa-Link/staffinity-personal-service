using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Domain.Modules.Employees.Ports.In
{
    public interface IGetAllEmployeesUseCase
    {
        Task<Employee[]> GetAllAsync();
    }
}
