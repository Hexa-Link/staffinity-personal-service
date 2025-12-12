using Staffinity.Personal.Domain.Modules.Vacations.Model;

namespace Staffinity.Personal.Domain.Modules.Vacations.Ports.In;

public interface IGetVacationRequestByIdUseCase
{
    Task<VacationRequest?> GetByIdAsync(VacationRequestId id);
}