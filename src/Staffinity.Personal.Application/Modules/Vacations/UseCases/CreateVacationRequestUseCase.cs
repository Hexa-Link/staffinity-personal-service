using Staffinity.Personal.Domain.Modules.Vacations.Model;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.In;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Vacations.UseCases
{
    public class CreateVacationRequestUseCase : ICreateVacationRequestUseCase
    {
        private readonly IVacationRequestRepository _repository;

        public CreateVacationRequestUseCase(IVacationRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<VacationRequest> CreateAsync(VacationRequest vacationRequest)
        {
            
            if (vacationRequest == null)
            {
                throw new ArgumentNullException(nameof(vacationRequest), "Vacation request cannot be null");
            }

            if (vacationRequest.EmployeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(vacationRequest.EmployeeId), "Employee Id cannot be empty");
            }
            
            await _repository.SaveAsync(vacationRequest);

            return vacationRequest;
        }
    }
}