using Staffinity.Personal.Domain.Modules.Vacations.Model;

namespace Staffinity.Personal.Infrastructure.Persistence.Vacations
{
    public static class VacationRequestMapper
    {
        // Convierte de la Entidad de BD (Plana) al Modelo de Dominio (Rico)
        public static VacationRequest ToModel(VacationRequestEntity entity)
        {
            // Parseo seguro del Enum (Si falla, asume Pending)
            if (!Enum.TryParse<VacationStatus>(entity.Status, true, out var statusEnum))
            {
                statusEnum = VacationStatus.Pending;
            }

            // Usamos el constructor de "Historia" (Rehidratación)
            return new VacationRequest(
                new VacationRequestId(entity.Id),
                entity.EmployeeId,
                entity.StartDate,
                entity.EndDate,
                entity.Reason,
                statusEnum, // Mantiene el estado real de la BD
                entity.CreatedAt // Mantiene la fecha real de creación
            );
        }

        // Convierte del Modelo de Dominio a la Entidad de BD
        public static VacationRequestEntity ToEntity(VacationRequest model)
        {
            return new VacationRequestEntity(
                model.Id.Value,
                model.EmployeeId,
                model.StartDate,
                model.EndDate,
                model.Reason,
                model.Status.ToString(), // Guarda el Enum como texto
                model.CreatedAt
            );
        }

        // Helper para listas (De BD a Dominio)
        public static VacationRequest[] ToModelList(List<VacationRequestEntity> entities)
        {
            return entities.Select(ToModel).ToArray();
        }

        // Helper para listas (De Dominio a BD) - Agregado para consistencia con Notifications
        public static List<VacationRequestEntity> ToEntityList(IEnumerable<VacationRequest> models)
        {
            return models.Select(ToEntity).ToList();
        }
    }
}
