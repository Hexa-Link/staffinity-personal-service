using Microsoft.AspNetCore.Mvc;
using Staffinity.Personal.Api.Modules.Vacations.Mappers;
using Staffinity.Personal.Application.Modules.Vacations.Dto;
// NOTA: Asegúrate de que tu namespace de DTOs sea igual aquí y en el Mapper (DTOs vs Dto).
// Si tu carpeta se llama DTOs, cambia la línea de arriba a .DTOs;

using Staffinity.Personal.Domain.Modules.Vacations.Exceptions;
using Staffinity.Personal.Domain.Modules.Vacations.Model;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.In;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.Out;

namespace Staffinity.Personal.Api.Modules.Vacations.Controllers
{
    [ApiController]
    [Route("vacation-requests")]
    public class VacationsRequestsController : ControllerBase
    {
        private readonly ICreateVacationRequestUseCase _createVacationRequestUseCase;
        private readonly IApproveVacationUseCase _approveVacationUseCase;
        private readonly IRejectVacationUseCase _rejectVacationUseCase;

        public VacationsRequestsController(
            ICreateVacationRequestUseCase createVacationRequestUseCase,
            IApproveVacationUseCase approveVacationUseCase,
            IRejectVacationUseCase rejectVacationUseCase
        )
        {
            _createVacationRequestUseCase = createVacationRequestUseCase;
            _approveVacationUseCase = approveVacationUseCase;
            _rejectVacationUseCase = rejectVacationUseCase;
        }

        // Create a new vacation request
        [HttpPost]
        [ProducesResponseType(typeof(VacationRequestResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] CreateVacationRequestDto request)
        {
            try
            {
                // CORREGIDO: Usando el nombre exacto de tu Mapper
                var entity = VacationRequestMapper.CreateDtoToModel(request);

                // Call the Use Case
                var createdEntity = await _createVacationRequestUseCase.CreateAsync(entity);

                // Convert Domain Entity back to Response DTO
                var response = VacationRequestMapper.ModelToResponse(createdEntity);

                // Return 201 Created
                return CreatedAtAction(nameof(Create), new { id = response.Id }, response);
            }
            catch (InvalidVacationDateException ex)
            {
                // Business Rule: Invalid dates (Start date in past, End before Start)
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal error occurred while creating the request.");
            }
        }

        // Approve a vacation request
        [HttpPut("{id}/approve")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Approve(string id)
        {
            if (!Guid.TryParse(id, out var vacationIdGuid))
                return BadRequest("The provided id is not a valid Guid");

            try
            {
                var vacationRequestId = new VacationRequestId(vacationIdGuid);

                await _approveVacationUseCase.ApproveAsync(vacationRequestId);

                return Ok("Vacation request approved successfully");
            }
            catch (VacationRequestNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Business Rule: Only pending requests can be approved
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Reject a vacation request
        [HttpPut("{id}/reject")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Reject(string id)
        {
            if (!Guid.TryParse(id, out var vacationIdGuid))
                return BadRequest("The provided id is not a valid Guid");

            try
            {
                var vacationRequestId = new VacationRequestId(vacationIdGuid);

                await _rejectVacationUseCase.RejectAsync(vacationRequestId);

                return Ok("Vacation request rejected successfully");
            }
            catch (VacationRequestNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                // Business Rule: Only pending requests can be rejected
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
