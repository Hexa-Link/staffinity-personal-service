using Staffinity.Personal.Domain.Modules.Vacations.Model;

namespace Staffinity.Personal.Domain.Modules.Vacations.Ports.In;

public interface IGetEmployeeVacationsUseCase
{
    Task<VacationRequest[]> GetByEmployeeIdAsync(Guid employeeId);
}