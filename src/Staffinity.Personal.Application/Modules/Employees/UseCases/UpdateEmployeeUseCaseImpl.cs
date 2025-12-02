using System.Linq;
using System.Net.Mail;
using Staffinity.Personal.Domain.Modules.Employees.Exceptions;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Employees.UseCases;

public class UpdateEmployeeUseCaseImpl : IUpdateEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;

    public UpdateEmployeeUseCaseImpl(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
    }

    public async Task<Employee?> UpdateAsync(Employee employee)
    {
        ArgumentNullException.ThrowIfNull(employee);
        ValidateEmployee(employee);

        // Buscar el empleado existente
        var existing = await _employeeRepository.GetByIdAsync(employee.Id)
            .ConfigureAwait(false);

        if (existing is null)
            throw new InvalidOperationException("Employee not found.");

        // Validar duplicado de email
        var allEmployees = await _employeeRepository.GetAllAsync().ConfigureAwait(false)
                           ?? Array.Empty<Employee>();

        if (allEmployees.Any(e =>
                e.Id != employee.Id &&
                string.Equals(e.Email, employee.Email, StringComparison.OrdinalIgnoreCase)))
        {
            throw new InvalidValueException($"An employee with email '{employee.Email}' already exists.");
        }

        // Construir el empleado actualizado
        var updatedEmployee = new Employee(
            existing.Id,
            employee.Code,
            employee.Name,
            employee.Email,
            employee.PasswordHash,
            employee.Phone,
            employee.BirthDate,
            employee.HireDate,
            employee.IdentificationTypeId,
            employee.IdentificationNumber,
            employee.ManagerId,
            employee.HeadquartersId,
            employee.GenderId,
            employee.StatusId,
            employee.AccessLevelId,
            existing.CreatedAt,
            DateTimeOffset.UtcNow,
            existing.IsDeleted
        );

        var savedEmployee = await _employeeRepository.UpdateAsync(updatedEmployee)
            .ConfigureAwait(false);

        if (savedEmployee is null)
            throw new InvalidOperationException("Employee could not be updated.");

        return savedEmployee;
    }

    private static void ValidateEmployee(Employee employee)
    {
        if (employee.Id == Guid.Empty)
            throw new InvalidValueException("Employee id is required.");

        if (string.IsNullOrWhiteSpace(employee.Code))
            throw new InvalidValueException("Code is required.");

        if (string.IsNullOrWhiteSpace(employee.Name))
            throw new InvalidValueException("Name is required.");

        if (string.IsNullOrWhiteSpace(employee.Email))
            throw new InvalidValueException("Email is required.");

        if (!IsValidEmail(employee.Email))
            throw new InvalidValueException("Invalid email format.");

        if (string.IsNullOrWhiteSpace(employee.PasswordHash))
            throw new InvalidValueException("Password hash is required.");

        if (string.IsNullOrWhiteSpace(employee.Phone))
            throw new InvalidValueException("Phone is required.");

        if (string.IsNullOrWhiteSpace(employee.IdentificationNumber))
            throw new InvalidValueException("Identification number is required.");

        if (employee.IdentificationTypeId == Guid.Empty)
            throw new InvalidValueException("Identification type is required.");

        if (employee.HeadquartersId == Guid.Empty)
            throw new InvalidValueException("Headquarters is required.");

        if (employee.GenderId == Guid.Empty)
            throw new InvalidValueException("Gender is required.");

        if (employee.StatusId == Guid.Empty)
            throw new InvalidValueException("Status is required.");

        if (employee.AccessLevelId == Guid.Empty)
            throw new InvalidValueException("Access level is required.");

        if (employee.BirthDate > DateOnly.FromDateTime(DateTime.UtcNow))
            throw new InvalidValueException("Birth date cannot be in the future.");

        if (employee.HireDate < employee.BirthDate)
            throw new InvalidValueException("Hire date cannot be before birth date.");
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
