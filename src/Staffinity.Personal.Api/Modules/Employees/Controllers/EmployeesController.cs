using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Staffinity.Personal.Application.Modules.Employees.Dtos;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Auth.Model;

namespace Staffinity.Personal.Api.Modules.Employees.Controllers;

[ApiController]
[Route("employees")]
public class EmployeesController : ControllerBase
{
    private readonly ICreateEmployeeUseCase _createEmployeeUseCase;
    private readonly IGetAllEmployeesUseCase _getEmployeesUseCase;
    private readonly IGetEmployeeByIdUseCase _getEmployeeByIdUseCase;
    private readonly IUpdateEmployeeUseCase _updateEmployeeUseCase;
    private readonly IDeleteEmployeeUseCase _deleteEmployeeUseCase;

    public EmployeesController(
        ICreateEmployeeUseCase createEmployeeUseCase,
        IGetAllEmployeesUseCase getEmployeesUseCase,
        IGetEmployeeByIdUseCase getEmployeeByIdUseCase,
        IUpdateEmployeeUseCase updateEmployeeUseCase,
        IDeleteEmployeeUseCase deleteEmployeeUseCase)
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

        if (string.IsNullOrWhiteSpace(request.Password))
            return BadRequest("Password is required.");

        var passwordHash = HashPassword(request.Password);

        // Usamos el ctor de dominio que genera el Id y CreatedAt/UpdatedAt
        var employee = new Employee(
            request.Code,
            request.FirstName,
            request.MiddleName,
            request.LastName,
            request.SecondLastName,
            request.Email,
            passwordHash,
            request.PhoneNumber,
            request.DateOfBirth,
            request.HireDate,
            request.IdentificationTypeId,
            request.IdentificationNumber,
            request.ManagerId,
            request.HeadquartersId,
            request.GenderId,
            request.StatusId,
            request.AccessLevelId
        );

        var createdEmployee = await _createEmployeeUseCase.CreateAsync(employee);

        if (createdEmployee is null)
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

        var existing = await _getEmployeeByIdUseCase.GetByIdAsync(id);
        if (existing is null)
            return NotFound();

        var code = string.IsNullOrWhiteSpace(request.Code) ? existing.Code : request.Code;
        var firstName = string.IsNullOrWhiteSpace(request.FirstName) ? existing.FirstName : request.FirstName;
        var middleName = request.MiddleName ?? existing.MiddleName;
        var lastName = string.IsNullOrWhiteSpace(request.LastName) ? existing.LastName : request.LastName;
        var secondLastName = request.SecondLastName ?? existing.SecondLastName;
        var email = string.IsNullOrWhiteSpace(request.Email) ? existing.Email : request.Email;
        var phoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? existing.PhoneNumber : request.PhoneNumber;
        var dateOfBirth = request.DateOfBirth ?? existing.DateOfBirth;
        var hireDate = request.HireDate ?? existing.HireDate;
        var identificationTypeId = request.IdentificationTypeId ?? existing.IdentificationTypeId;
        var identificationNumber = string.IsNullOrWhiteSpace(request.IdentificationNumber)
            ? existing.IdentificationNumber
            : request.IdentificationNumber;
        var managerId = request.ManagerId ?? existing.ManagerId;
        var headquartersId = request.HeadquartersId ?? existing.HeadquartersId;
        var genderId = request.GenderId ?? existing.GenderId;
        var statusId = request.StatusId ?? existing.StatusId;
        var accessLevelId = request.AccessLevelId ?? existing.AccessLevelId;

        var employeeToUpdate = new Employee(
            id,
            code,
            firstName,
            middleName,
            lastName,
            secondLastName,
            email,
            existing.PasswordHash,
            phoneNumber,
            dateOfBirth,
            hireDate,
            identificationTypeId,
            identificationNumber,
            managerId,
            headquartersId,
            genderId,
            statusId,
            accessLevelId,
            existing.CreatedAt,
            existing.IsDeleted
        );

        var updatedEmployee = await _updateEmployeeUseCase.UpdateAsync(employeeToUpdate);

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
                employee.FirstName,
                employee.MiddleName,
                employee.LastName,
                employee.SecondLastName,
                employee.Email,
                employee.PhoneNumber,
                employee.DateOfBirth,
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
