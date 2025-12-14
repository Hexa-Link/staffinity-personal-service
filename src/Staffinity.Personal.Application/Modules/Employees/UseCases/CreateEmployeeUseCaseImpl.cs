using FluentValidation;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.In;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Employees.UseCases;

public class CreateEmployeeUseCaseImpl : ICreateEmployeeUseCase
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IValidator<Employee> _validator;

    public CreateEmployeeUseCaseImpl(
        IEmployeeRepository employeeRepository,
        IValidator<Employee> validator)
    {
        _employeeRepository = employeeRepository 
            ?? throw new ArgumentNullException(nameof(employeeRepository));

        _validator = validator 
            ?? throw new ArgumentNullException(nameof(validator));
    }

    public async Task<Employee?> CreateAsync(Employee employee)
    {
        if (employee is null)
        {
            Console.WriteLine("Error: Employee is null.");
            return null;
        }

        var validationResult = await _validator.ValidateAsync(employee);
        if (!validationResult.IsValid)
        {
            Console.WriteLine("Error: Employee data is invalid.");
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine($"- {error.ErrorMessage}");
            }
            return null;
        }

        var allEmployees = await _employeeRepository.GetAllAsync() ?? Array.Empty<Employee>();

        if (allEmployees.Any(e =>
            string.Equals(e.Email, employee.Email, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine($"Error: The email '{employee.Email}' is already registered.");
            return null;
        }

        try
        {
            var created = await _employeeRepository.CreateAsync(employee);

            if (created is null)
            {
                Console.WriteLine("Error: Employee could not be created.");
                return null;
            }

            Console.WriteLine($"Success: Employee '{employee.FirstName} {employee.LastName}' created.");
            return created;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving employee: {ex.Message}");
            return null;
        }
    }
}
