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
        _createEmployeeUseCase = createEmployeeUseCase;
        _getEmployeesUseCase = getEmployeesUseCase;
        _getEmployeeByIdUseCase = getEmployeeByIdUseCase;
        _updateEmployeeUseCase = updateEmployeeUseCase;
        _deleteEmployeeUseCase = deleteEmployeeUseCase;
    }
    
    [HttpPost]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeRequest request)
    {
        if (request is null)
            return BadRequest("Request body is required.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var passwordHash = HashPassword(request.Password);

        var command = new CreateEmployeeCommand(
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
            request.AccessLevelId
        );

        var employee = await _createEmployeeUseCase.ExecuteAsync(command);
        var response = MapToResponse(employee);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }
    
    [HttpGet]
    [ProducesResponseType(typeof(EmployeeResponse[]), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var employees = await _getEmployeesUseCase.ExecuteAsync();
        var responses = employees.Select(MapToResponse).ToArray();
        return Ok(responses);
    }
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id)
    {
        var employee = await _getEmployeeByIdUseCase.ExecuteAsync(id);

        if (employee is null)
            return NotFound();

        return Ok(MapToResponse(employee));
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(EmployeeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEmployeeRequest request)
    {
        if (request is null)
            return BadRequest("Request body is required.");

        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var passwordHash = HashPassword(request.Password);

        var command = new UpdateEmployeeCommand(
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
            request.AccessLevelId
        );

        var employee = await _updateEmployeeUseCase.ExecuteAsync(command);
        return Ok(MapToResponse(employee));
    }
    
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _deleteEmployeeUseCase.ExecuteAsync(id);

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
