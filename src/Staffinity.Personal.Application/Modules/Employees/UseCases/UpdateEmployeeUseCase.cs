using System.Linq;
using System.Net.Mail;
using Staffinity.Personal.Domain.Modules.Employees.Exceptions;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Employees.UseCases;

public class UpdateEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateEmployeeUseCase(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    }

    public async Task<Employee> ExecuteAsync(UpdateEmployeeCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ValidateCommand(command);

        var existing = await _employeeRepository.GetByIdAsync(command.Id).ConfigureAwait(false);
        if (existing is null)
        {
            throw new InvalidValueException("Employee not found.");
        }

        var employees = await _employeeRepository.GetAllAsync().ConfigureAwait(false)
                        ?? Array.Empty<Employee>();
        if (employees.Any(e => e.Id != command.Id && string.Equals(e.Email, command.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidValueException($"An employee with email '{command.Email}' already exists.");
        }

        var updatedEmployee = new Employee(
            existing.Id,
            command.Code,
            command.Name,
            command.Email,
            command.PasswordHash,
            command.Phone,
            command.BirthDate,
            command.HireDate,
            command.IdentificationTypeId,
            command.IdentificationNumber,
            command.ManagerId,
            command.HeadquartersId,
            command.GenderId,
            command.StatusId,
            command.AccessLevelId,
            existing.CreatedAt,
            DateTimeOffset.UtcNow,
            existing.IsDeleted);

        var savedEmployee = await _employeeRepository.UpdateAsync(updatedEmployee).ConfigureAwait(false);
        if (savedEmployee is null)
        {
            throw new InvalidOperationException("Employee could not be updated.");
        }

        return savedEmployee;
    }

    private static void ValidateCommand(UpdateEmployeeCommand command)
    {
        if (command.Id == Guid.Empty)
        {
            throw new InvalidValueException("Employee id is required.");
        }

        if (string.IsNullOrWhiteSpace(command.Code))
        {
            throw new InvalidValueException("Code is required.");
        }

        if (string.IsNullOrWhiteSpace(command.Name))
        {
            throw new InvalidValueException("Name is required.");
        }

        if (string.IsNullOrWhiteSpace(command.Email))
        {
            throw new InvalidValueException("Email is required.");
        }

        if (!IsValidEmail(command.Email))
        {
            throw new InvalidValueException("Email format is invalid.");
        }

        if (string.IsNullOrWhiteSpace(command.PasswordHash))
        {
            throw new InvalidValueException("Password hash is required.");
        }

        if (string.IsNullOrWhiteSpace(command.Phone))
        {
            throw new InvalidValueException("Phone is required.");
        }

        if (string.IsNullOrWhiteSpace(command.IdentificationNumber))
        {
            throw new InvalidValueException("Identification number is required.");
        }

        if (command.IdentificationTypeId == Guid.Empty)
        {
            throw new InvalidValueException("Identification type is required.");
        }

        if (command.HeadquartersId == Guid.Empty)
        {
            throw new InvalidValueException("Headquarters is required.");
        }

        if (command.GenderId == Guid.Empty)
        {
            throw new InvalidValueException("Gender is required.");
        }

        if (command.StatusId == Guid.Empty)
        {
            throw new InvalidValueException("Status is required.");
        }

        if (command.AccessLevelId == Guid.Empty)
        {
            throw new InvalidValueException("Position (access level) is required.");
        }

        if (command.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow))
        {
            throw new InvalidValueException("Birth date cannot be in the future.");
        }

        if (command.HireDate < command.BirthDate)
        {
            throw new InvalidValueException("Hire date cannot be earlier than birth date.");
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}

public record UpdateEmployeeCommand(
    Guid Id,
    string Code,
    string Name,
    string Email,
    string PasswordHash,
    string Phone,
    DateOnly BirthDate,
    DateOnly HireDate,
    Guid IdentificationTypeId,
    string IdentificationNumber,
    Guid? ManagerId,
    Guid HeadquartersId,
    Guid GenderId,
    Guid StatusId,
    Guid AccessLevelId);
