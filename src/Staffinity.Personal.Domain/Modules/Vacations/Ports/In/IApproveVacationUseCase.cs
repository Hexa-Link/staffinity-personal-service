using Staffinity.Personal.Domain.Modules.Vacations.Model;

namespace Staffinity.Personal.Domain.Modules.Vacations.Ports.In
{
    public interface IApproveVacationUseCase
    {
        Task<bool> ApproveAsync(VacationRequestId id);
    }
}