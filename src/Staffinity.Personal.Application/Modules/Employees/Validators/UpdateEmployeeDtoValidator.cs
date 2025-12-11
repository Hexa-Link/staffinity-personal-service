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

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(x => x.PasswordHash)
            .NotEmpty().WithMessage("Password hash is required.");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required.");

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

        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
            .WithMessage("Birth date cannot be in the future.");

        RuleFor(x => x.HireDate)
            .GreaterThanOrEqualTo(x => x.BirthDate)
            .WithMessage("Hire date cannot be before birth date.");
    }
}
