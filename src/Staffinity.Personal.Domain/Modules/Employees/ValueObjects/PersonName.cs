using Staffinity.Personal.Domain.Modules.Employees.Exceptions;

namespace Staffinity.Personal.Domain.Modules.Employees.ValueObjects;

public sealed record PersonName
{
    public string FirstName { get; }
    public string? MiddleName { get; }
    public string LastName { get; }
    public string? SecondLastName { get; }

    private PersonName(string firstName, string? middleName, string lastName, string? secondLastName)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        SecondLastName = secondLastName;
    }

    public static PersonName Create(string firstName, string? middleName, string lastName, string? secondLastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new EmptyNameException();

        if (string.IsNullOrWhiteSpace(lastName))
            throw new EmptyNameException();

        var fn = firstName.Trim();
        var mn = string.IsNullOrWhiteSpace(middleName) ? null : middleName.Trim();
        var ln = lastName.Trim();
        var sln = string.IsNullOrWhiteSpace(secondLastName) ? null : secondLastName.Trim();

        ValidateLength(fn, nameof(firstName));
        if (mn is not null) ValidateLength(mn, nameof(middleName));
        ValidateLength(ln, nameof(lastName));
        if (sln is not null) ValidateLength(sln, nameof(secondLastName));

        return new PersonName(fn, mn, ln, sln);
    }

    private static void ValidateLength(string value, string fieldName)
    {
        if (value.Length > 40)
            throw new InvalidValueException($"{fieldName} cannot exceed 40 characters.");
    }

    public override string ToString()
    {
        var parts = new[]
        {
            FirstName,
            MiddleName,
            LastName,
            SecondLastName
        };

        return string.Join(' ', parts.Where(p => !string.IsNullOrWhiteSpace(p)));
    }
}
