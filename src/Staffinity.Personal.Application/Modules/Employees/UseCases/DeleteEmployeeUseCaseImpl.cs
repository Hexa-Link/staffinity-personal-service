using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Employees.UseCases;

public class DeleteEmployeeUseCaseImpl : IDeleteEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public DeleteEmployeeUseCaseImpl(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    }

    public async Task<bool> DeleteAsync(Guid employeeId)
    {
        if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException("Employee Id cannot be null or empty");
            }

            return await _employeeRepository.DeleteAsync(employeeId);
    }
}
