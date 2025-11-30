using System.ComponentModel.DataAnnotations;

namespace Staffinity.Personal.Application.Modules.Employees.Dtos;

public class UpdateEmployeeRequest
{
    [Required]
    public string Code { get; set; }

    [Required]
    public string Name { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Phone { get; set; }

    [Required]
    public DateOnly BirthDate { get; set; }

    [Required]
    public DateOnly HireDate { get; set; }

    [Required]
    public Guid IdentificationTypeId { get; set; }

    [Required]
    public string IdentificationNumber { get; set; }

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
