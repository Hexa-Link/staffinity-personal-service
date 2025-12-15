using System.ComponentModel.DataAnnotations;

namespace Staffinity.Personal.Application.Modules.Employees.Dtos;

public class UpdateEmployeeRequest
{
    public string Code { get; set; } = default!;
    public string FirstName { get; set; } = default!;
    public string? MiddleName { get; set; }
    public string LastName { get; set; } = default!;
    public string? SecondLastName { get; set; }
    public string Email { get; set; } = default!;
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public DateTime? HireDate { get; set; }
    public Guid? IdentificationTypeId { get; set; }
    public string? IdentificationNumber { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid? HeadquartersId { get; set; }
    public Guid? GenderId { get; set; }
    public Guid? StatusId { get; set; }
    public Guid? AccessLevelId { get; set; }
}
