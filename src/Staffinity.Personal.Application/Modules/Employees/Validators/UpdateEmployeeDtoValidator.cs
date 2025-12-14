using System;
using FluentValidation;
using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Application.Modules.Employees.Validators;

public class UpdateEmployeeDtoValidator : AbstractValidator<Employee>
{
    public UpdateEmployeeDtoValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Employee id is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.");

        RuleFor(x => x.IdentificationNumber)
            .NotEmpty().WithMessage("Identification number is required.");

        RuleFor(x => x.IdentificationTypeId)
            .NotEmpty().WithMessage("Identification type is required.");

        RuleFor(x => x.HeadquartersId)
            .NotEmpty().WithMessage("Headquarters is required.");

        RuleFor(x => x.GenderId)
            .NotEmpty().WithMessage("Gender is required.");

        RuleFor(x => x.StatusId)
            .NotEmpty().WithMessage("Status is required.");

        RuleFor(x => x.AccessLevelId)
            .NotEmpty().WithMessage("Access level is required.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Date of birth cannot be in the future.");

        RuleFor(x => x.HireDate)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.DateOfBirth)
            .WithMessage("Hire date cannot be before birth date.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Hire date cannot be in the future.");
    }
}
