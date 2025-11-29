using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Employees.UseCases;

public class DeleteEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeUseCase(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    }

    public async Task<bool> ExecuteAsync(Guid employeeId)
    {
        if (employeeId == Guid.Empty)
        {
            throw new ArgumentException("Employee id is required.", nameof(employeeId));
        }

        return await _employeeRepository.DeleteAsync(employeeId).ConfigureAwait(false);
    }
}
