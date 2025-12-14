using System.ComponentModel.DataAnnotations;

namespace Staffinity.Personal.Application.Modules.Employees.Dtos;

public class CreateEmployeeRequest
{
    [Required]
    public string Code { get; set; } = string.Empty;

    [Required]
    public string FirstName { get; set; } = string.Empty;

    public string? MiddleName { get; set; }

    [Required]
    public string LastName { get; set; } = string.Empty;

    public string? SecondLastName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    [Required]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    [Required]
    public Guid IdentificationTypeId { get; set; }

    [Required]
    public string IdentificationNumber { get; set; } = string.Empty;

    public Guid? ManagerId { get; set; }

    [Required]
    public Guid HeadquartersId { get; set; }

    [Required]
    public Guid GenderId { get; set; }

    [Required]
    public Guid StatusId { get; set; }

    [Required]
    public Guid AccessLevelId { get; set; }
}
