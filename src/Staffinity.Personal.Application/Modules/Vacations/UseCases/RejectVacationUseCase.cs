using Staffinity.Personal.Domain.Modules.Vacations.Model;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.In;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Vacations.UseCases
{
    public class RejectVacationUseCase : IRejectVacationUseCase
    {
        private readonly IVacationRequestRepository _repository;

        public RejectVacationUseCase(IVacationRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> RejectAsync(VacationRequestId id)
        {
            var request = await _repository.GetByIdAsync(id);

            if (request == null)
            {
                throw new VacationRequestNotFoundException(
                    $"Vacation request with id {id.Value} not found."
                );
            }

            request.Reject();

            await _repository.UpdateAsync(request);

            return true;
        }
    }
}
