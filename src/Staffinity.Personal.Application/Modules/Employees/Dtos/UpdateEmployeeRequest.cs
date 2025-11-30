using System.ComponentModel.DataAnnotations;

namespace Staffinity.Personal.Application.Modules.Employees.Dtos;

public class UpdateEmployeeRequest
{
    [Required]
    public required string Code { get; set; } = string.Empty;

    [Required]
    public required string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public required string Email { get; set; } = string.Empty;

    [Required]
    public required string Password { get; set; } = string.Empty;

    [Required]
    public required string Phone { get; set; } = string.Empty;

    [Required]
    public DateOnly BirthDate { get; set; }

    [Required]
    public DateOnly HireDate { get; set; }

    [Required]
    public Guid IdentificationTypeId { get; set; }

    [Required]
    public required string IdentificationNumber { get; set; } = string.Empty;

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
