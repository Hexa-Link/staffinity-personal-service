using Staffinity.Personal.Domain.Modules.Vacations.Model;

namespace Staffinity.Personal.Infrastructure.Persistence.Vacations
{
    public static class VacationRequestMapper
    {
        public static VacationRequest ToModel(VacationRequestEntity entity)
        {
            if (!Enum.TryParse<VacationStatus>(entity.Status, true, out var statusEnum))
            {
                statusEnum = VacationStatus.Pending;
            }

            // Constructor for History load
            return new VacationRequest(
                new VacationRequestId(entity.Id),
                entity.EmployeeId,
                entity.StartDate,
                entity.EndDate,
                entity.Reason,
                statusEnum,       
                entity.CreatedAt 
            );
        }

        public static VacationRequestEntity ToEntity(VacationRequest model)
        {
            return new VacationRequestEntity(
                model.Id.Value,
                model.EmployeeId,
                model.StartDate,
                model.EndDate,
                model.Reason,
                model.Status.ToString(), 
                model.CreatedAt
            );
        }

        public static VacationRequest[] ToModelList(List<VacationRequestEntity> entities)
        {
            return entities.Select(ToModel).ToArray();
        }
    }
}
