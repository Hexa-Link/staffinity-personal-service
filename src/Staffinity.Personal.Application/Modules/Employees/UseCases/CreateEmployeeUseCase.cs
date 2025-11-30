using System.Linq;
using System.Net.Mail;
using Staffinity.Personal.Domain.Modules.Employees.Exceptions;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Employees.UseCases;

public class CreateEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public CreateEmployeeUseCase(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    }

    public async Task<Employee> ExecuteAsync(CreateEmployeeCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ValidateCommand(command);

        var existingEmployees = await _employeeRepository.GetAllAsync().ConfigureAwait(false)
                               ?? Array.Empty<Employee>();
        if (existingEmployees.Any(e => string.Equals(e.Email, command.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidValueException($"An employee with email '{command.Email}' already exists.");
        }

        var employee = new Employee(
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
            command.AccessLevelId);

        var createdEmployee = await _employeeRepository.CreateAsync(employee).ConfigureAwait(false);
        if (createdEmployee is null)
        {
            throw new InvalidOperationException("Employee could not be created.");
        }

        return createdEmployee;
    }

    private static void ValidateCommand(CreateEmployeeCommand command)
    {
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

public record CreateEmployeeCommand(
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
