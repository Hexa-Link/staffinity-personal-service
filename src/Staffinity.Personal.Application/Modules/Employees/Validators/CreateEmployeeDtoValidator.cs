using FluentValidation;
using Staffinity.Personal.Application.Modules.Employees.Dtos;

namespace Staffinity.Personal.Application.Modules.Employees.Validators
{
    public class CreateEmployeeDtoValidator : AbstractValidator<CreateEmployeeRequest>
    {
        public CreateEmployeeDtoValidator()
        {
            RuleFor(e => e.Code)
                .NotEmpty()
                .WithMessage("Code is required.");

            RuleFor(e => e.Name)
                .NotEmpty()
                .WithMessage("Name is required.");

            RuleFor(e => e.Email)
                .NotEmpty()
                .WithMessage("Email is required.")
                .EmailAddress()
                .WithMessage("Email format is invalid.");

            RuleFor(e => e.Phone)
                .NotEmpty()
                .WithMessage("Phone is required.");

            RuleFor(e => e.IdentificationNumber)
                .NotEmpty()
                .WithMessage("Identification number is required.");

            RuleFor(e => e.IdentificationTypeId)
                .NotEmpty()
                .WithMessage("Identification type is required.");

            RuleFor(e => e.HeadquartersId)
                .NotEmpty()
                .WithMessage("Headquarters is required.");

            RuleFor(e => e.GenderId)
                .NotEmpty()
                .WithMessage("Gender is required.");

            RuleFor(e => e.StatusId)
                .NotEmpty()
                .WithMessage("Status is required.");

            RuleFor(e => e.AccessLevelId)
                .NotEmpty()
                .WithMessage("Position (access level) is required.");

            RuleFor(e => e.BirthDate)
                .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Birth date cannot be in the future.");

            RuleFor(e => e.HireDate)
                .GreaterThan(e => e.BirthDate)
                .WithMessage("Hire date cannot be earlier than birth date.");
        }
    }
}
