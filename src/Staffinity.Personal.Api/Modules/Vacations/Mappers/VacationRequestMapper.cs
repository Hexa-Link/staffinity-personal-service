using Staffinity.Personal.Application.Modules.Vacations.DTOs;
using Staffinity.Personal.Domain.Modules.Vacations.Model;

namespace Staffinity.Personal.Api.Modules.Vacations.Mappers
{
    public static class VacationRequestMapper
    {
        // 1. De DTO (Front) a Dominio (Entidad)
        // Se usa cuando llega un POST para crear.
        public static VacationRequest CreateDtoToModel(CreateVacationRequestDto dto)
        {
            // Aquí generamos el ID nuevo y usamos el constructor de "Creación"
            // (El que pone Status = Pending y Fecha = Hoy automáticamente)
            return new VacationRequest(
                new VacationRequestId(Guid.NewGuid()), // Generamos ID aquí
                dto.EmployeeId,
                dto.StartDate,
                dto.EndDate,
                dto.Reason
            );
        }

        // 2. De Dominio (Entidad) a DTO (Respuesta)
        // Se usa para responder al Front después de guardar.
        public static VacationRequestResponseDto ModelToResponse(VacationRequest model)
        {
            return new VacationRequestResponseDto
            {
                Id = model.Id.Value,
                EmployeeId = model.EmployeeId,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Status = model.Status.ToString(),
                Reason = model.Reason,
                CreatedAt = model.CreatedAt,
            };
        }
    }
}
