using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Staffinity.Personal.Application.Modules.Employees.Dtos;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly CreateEmployeeUseCase _createEmployeeUseCase;
    private readonly GetEmployeesUseCase _getEmployeesUseCase;

    public EmployeesController(
        CreateEmployeeUseCase createEmployeeUseCase,
        GetEmployeesUseCase getEmployeesUseCase)
    {
        _createEmployeeUseCase = createEmployeeUseCase ?? throw new ArgumentNullException(nameof(createEmployeeUseCase));
        _getEmployeesUseCase = getEmployeesUseCase ?? throw new ArgumentNullException(nameof(getEmployeesUseCase));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        if (request is null)
        {
            return BadRequest("Request body is required.");
        }

        var command = new CreateEmployeeCommand(
            request.Code,
            request.Name,
            request.Email,
            request.Password, // TODO: apply real password hashing here
            request.Phone,
            request.BirthDate,
            request.HireDate,
            request.IdentificationTypeId,
            request.IdentificationNumber,
            request.ManagerId,
            request.HeadquartersId,
            request.GenderId,
            request.StatusId,
            request.AccessLevelId);

        var employee = await _createEmployeeUseCase.ExecuteAsync(command).ConfigureAwait(false);

        var response = MapToResponse(employee);
        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _getEmployeesUseCase.ExecuteAsync().ConfigureAwait(false);
        var responses = employees.Select(MapToResponse).ToArray();
        return Ok(responses);
    }

    private static EmployeeResponse MapToResponse(Employee employee)
    {
        return new EmployeeResponse(
            employee.Id,
            employee.Code,
            employee.Name,
            employee.Email,
            employee.Phone,
            employee.BirthDate,
            employee.HireDate,
            employee.IdentificationTypeId,
            employee.IdentificationNumber,
            employee.ManagerId,
            employee.HeadquartersId,
            employee.GenderId,
            employee.StatusId,
            employee.AccessLevelId);
    }
}
