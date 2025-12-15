using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Employees.UseCases;

public class GetAllEmployeesUseCaseImpl : IGetAllEmployeesUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetAllEmployeesUseCaseImpl(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    }

    public async Task<Employee[]> GetAllAsync()
    {
        var employees = await _employeeRepository.GetAllAsync().ConfigureAwait(false);
        return employees ?? Array.Empty<Employee>();
    }
}
