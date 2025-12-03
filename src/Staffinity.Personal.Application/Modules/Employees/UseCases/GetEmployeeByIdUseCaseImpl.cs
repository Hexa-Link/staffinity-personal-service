using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Employees.UseCases;

public class GetEmployeeByIdUseCaseImpl : IGetEmployeeByIdUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public GetEmployeeByIdUseCaseImpl(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    }

    public async Task<Employee?> GetByIdAsync(Guid employeeId)
    {
        if (employeeId == Guid.Empty)
        {
            throw new ArgumentException("Employee id is required.", nameof(employeeId));
        }

        return await _employeeRepository.GetByIdAsync(employeeId);
    }
}
