using Staffinity.Personal.Domain.Modules.Vacations.Model;

namespace Staffinity.Personal.Domain.Modules.Vacations.Ports.In;

public interface ICreateVacationRequestUseCase
{
    Task<VacationRequest> CreateAsync(VacationRequest vacationRequest);
}