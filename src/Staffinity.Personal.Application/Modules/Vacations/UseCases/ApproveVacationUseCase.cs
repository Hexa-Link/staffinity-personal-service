using Staffinity.Personal.Domain.Modules.Vacations.Exceptions;
using Staffinity.Personal.Domain.Modules.Vacations.Model;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.In;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Vacations.UseCases
{
    public class ApproveVacationUseCase : IApproveVacationUseCase
    {
        private readonly IVacationRequestRepository _repository;

        public ApproveVacationUseCase(IVacationRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> ApproveAsync(VacationRequestId id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Vacation Request Id cannot be null");
            }

            var request = await _repository.GetByIdAsync(id);

            if (request == null)
            {
                throw new VacationRequestNotFoundException(
                    $"Vacation request with id {id.Value} not found."
                );
            }

            request.Approve();

            await _repository.UpdateAsync(request);

            return true;
        }
    }
}
