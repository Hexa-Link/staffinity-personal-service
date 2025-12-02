using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Staffinity.Personal.Application.Modules.Employees.Dtos;
using Staffinity.Personal.Application.Modules.Employees.UseCases;
using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Api.Controllers;

[ApiController]
[Route("employees")]
public class EmployeesController : ControllerBase
{
    private readonly CreateEmployeeUseCaseImpl _createEmployeeUseCase;
    private readonly GetAllEmployeesUseCaseImpl _getEmployeesUseCase;
    private readonly GetEmployeeByIdUseCaseImpl _getEmployeeByIdUseCase;
    private readonly UpdateEmployeeUseCaseImpl _updateEmployeeUseCase;
    private readonly DeleteEmployeeUseCaseImpl _deleteEmployeeUseCase;

    public EmployeesController(
        CreateEmployeeUseCaseImpl createEmployeeUseCase,
        GetAllEmployeesUseCaseImpl getEmployeesUseCase,
        GetEmployeeByIdUseCaseImpl getEmployeeByIdUseCase,
        UpdateEmployeeUseCaseImpl updateEmployeeUseCase,
        DeleteEmployeeUseCaseImpl deleteEmployeeUseCase)
    {
        _createEmployeeUseCase = createEmployeeUseCase;
        _getEmployeesUseCase = getEmployeesUseCase;
        _getEmployeeByIdUseCase = getEmployeeByIdUseCase;
        _updateEmployeeUseCase = updateEmployeeUseCase;
        _deleteEmployeeUseCase = deleteEmployeeUseCase;
    }

    [HttpPost]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        if (request is null)
            return BadRequest("Request body is required.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var passwordHash = HashPassword(request.Password);

        var now = DateTimeOffset.UtcNow;

        var employee = new Employee(
            Guid.NewGuid(),
            request.Code,
            request.Name,
            request.Email,
            passwordHash,
            request.Phone,
            request.BirthDate,
            request.HireDate,
            request.IdentificationTypeId,
            request.IdentificationNumber,
            request.ManagerId,
            request.HeadquartersId,
            request.GenderId,
            request.StatusId,
            request.AccessLevelId,
            now,       // createdAt
            now,       // updatedAt (no null)
            false      // isDeleted
        );

        var createdEmployee = await _createEmployeeUseCase.CreateAsync(employee);

        if (createdEmployee is null)
            // No pudo crearse por alguna razón, devuelve 500 (o maneja según tu política)
            return StatusCode(StatusCodes.Status500InternalServerError, "Employee could not be created.");

        var response = MapToResponse(createdEmployee);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpGet]
    [ProducesResponseType(typeof(EmployeeResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _getEmployeesUseCase.GetAllAsync();
        var responses = employees.Select(MapToResponse).ToArray();
        return Ok(responses);
    }


    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await _getEmployeeByIdUseCase.GetByIdAsync(id);

        if (employee is null)
            return NotFound();

        return Ok(MapToResponse(employee));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest request)
    {
        if (request is null)
            return BadRequest("Request body is required.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var passwordHash = HashPassword(request.Password);
        var now = DateTimeOffset.UtcNow;

        var employee = new Employee(
            id,
            request.Code,
            request.Name,
            request.Email,
            passwordHash,
            request.Phone,
            request.BirthDate,
            request.HireDate,
            request.IdentificationTypeId,
            request.IdentificationNumber,
            request.ManagerId,
            request.HeadquartersId,
            request.GenderId,
            request.StatusId,
            request.AccessLevelId,
            now,  
            now,   
            false  
        );

        var updatedEmployee = await _updateEmployeeUseCase.UpdateAsync(employee);

        if (updatedEmployee is null)
            return NotFound();

        return Ok(MapToResponse(updatedEmployee));
    }


    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _deleteEmployeeUseCase.DeleteAsync(id);

        if (!deleted)
            return NotFound();

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
            employee.AccessLevelId
        );
    }

    private static string HashPassword(string password)
    {
        var bytes = Encoding.UTF8.GetBytes(password);
        var hashBytes = SHA256.HashData(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}
