using Staffinity.Personal.Domain.Modules.Vacations.Model;

namespace Staffinity.Personal.Domain.Modules.Vacations.Ports.In;

public interface ICancelVacationsUseCase
{
    Task<bool> CancelAsync(VacationRequestId id);
}