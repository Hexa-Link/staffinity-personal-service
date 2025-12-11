using Staffinity.Personal.Domain.Modules.Vacations.Model;

namespace Staffinity.Personal.Domain.Modules.Vacations.Ports.Out;

public interface IVacationRequestRepository
{
    Task SaveAsync(VacationRequest vacationRequest);
    Task UpdateAsync(VacationRequest vacationRequest);
    Task DeleteAsync(VacationRequestId id);
    Task <VacationRequest?> GetByIdAsync (VacationRequestId id);
    Task<IEnumerable<VacationRequest>> GetByEmployeeIdAsync(Guid employeeId);
    Task<VacationRequest[]> GetAllAsync();
    
}
