using System.Linq;
using FluentValidation;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Employees.UseCases;

public class UpdateEmployeeUseCaseImpl : IUpdateEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IValidator<Employee> _validator;

    public UpdateEmployeeUseCaseImpl(
        IEmployeeRepository employeeRepository,
        IValidator<Employee> validator)
    {
        _employeeRepository = employeeRepository
            ?? throw new ArgumentNullException(nameof(employeeRepository));
        _validator = validator
            ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<Employee?> UpdateAsync(Employee employee)
    {
        if (employee is null)
        {
            Console.WriteLine("Error: Employee is null.");
            return null;
        }


        var validation = await _validator.ValidateAsync(employee).ConfigureAwait(false);

        if (!validation.IsValid)
        {
            Console.WriteLine("Error: Employee data is invalid.");
            foreach (var error in validation.Errors)
            {
                Console.WriteLine($"- {error.ErrorMessage}");
            }
            return null;
        }


        var existing = await _employeeRepository.GetByIdAsync(employee.Id)
            .ConfigureAwait(false);

        if (existing is null)
        {
            Console.WriteLine("Error: Employee not found.");
            return null;
        }


        var allEmployees = await _employeeRepository.GetAllAsync().ConfigureAwait(false)
                           ?? Array.Empty<Employee>();

        if (allEmployees.Any(e =>
                e.Id != employee.Id &&
                string.Equals(e.Email, employee.Email, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine($"Error: The email '{employee.Email}' is already registered.");
            return null;
        }


        try
        {
            var savedEmployee = await _employeeRepository.UpdateAsync(employee)
                .ConfigureAwait(false);

            if (savedEmployee is null)
            {
                Console.WriteLine("Error: Employee could not be updated.");
                return null;
            }

            Console.WriteLine($"Success: Employee '{employee.FirstName} {employee.LastName}' updated.");
            return savedEmployee;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating employee: {ex.Message}");
            return null;
        }
    }
}
