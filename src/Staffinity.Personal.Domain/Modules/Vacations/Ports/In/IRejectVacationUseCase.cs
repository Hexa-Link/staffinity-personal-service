using Staffinity.Personal.Domain.Modules.Vacations.Model;

namespace Staffinity.Personal.Domain.Modules.Vacations.Ports.In;

public interface IRejectVacationUseCase
{
    Task<bool> RejectAsync(VacationRequestId id);
}