using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Staffinity.Personal.Application.Modules.Employees.Dtos;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly CreateEmployeeUseCase _createEmployeeUseCase;
    private readonly GetEmployeesUseCase _getEmployeesUseCase;
    private readonly GetEmployeeByIdUseCase _getEmployeeByIdUseCase;
    private readonly UpdateEmployeeUseCase _updateEmployeeUseCase;
    private readonly DeleteEmployeeUseCase _deleteEmployeeUseCase;

    public EmployeesController(
        CreateEmployeeUseCase createEmployeeUseCase,
        GetEmployeesUseCase getEmployeesUseCase,
        GetEmployeeByIdUseCase getEmployeeByIdUseCase,
        UpdateEmployeeUseCase updateEmployeeUseCase,
        DeleteEmployeeUseCase deleteEmployeeUseCase)
    {
        _createEmployeeUseCase = createEmployeeUseCase ?? throw new ArgumentNullException(nameof(createEmployeeUseCase));
        _getEmployeesUseCase = getEmployeesUseCase ?? throw new ArgumentNullException(nameof(getEmployeesUseCase));
        _getEmployeeByIdUseCase = getEmployeeByIdUseCase ?? throw new ArgumentNullException(nameof(getEmployeeByIdUseCase));
        _updateEmployeeUseCase = updateEmployeeUseCase ?? throw new ArgumentNullException(nameof(updateEmployeeUseCase));
        _deleteEmployeeUseCase = deleteEmployeeUseCase ?? throw new ArgumentNullException(nameof(deleteEmployeeUseCase));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var command = new CreateEmployeeCommand(
            request.Code,
            request.Name,
            request.Email,
            request.PasswordHash,
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
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _getEmployeesUseCase.ExecuteAsync().ConfigureAwait(false);
        var responses = employees.Select(MapToResponse).ToArray();
        return Ok(responses);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await _getEmployeeByIdUseCase.ExecuteAsync(id).ConfigureAwait(false);
        if (employee is null)
        {
            return NotFound();
        }

        return Ok(MapToResponse(employee));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest request)
    {
        if (request is null)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var command = new UpdateEmployeeCommand(
            id,
            request.Code,
            request.Name,
            request.Email,
            request.PasswordHash,
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

        var employee = await _updateEmployeeUseCase.ExecuteAsync(command).ConfigureAwait(false);
        var response = MapToResponse(employee);
        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _deleteEmployeeUseCase.ExecuteAsync(id).ConfigureAwait(false);
        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
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
