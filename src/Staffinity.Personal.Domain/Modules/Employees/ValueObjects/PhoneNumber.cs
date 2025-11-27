using Staffinity.Personal.Domain.Modules.Employees.Exceptions;

namespace Staffinity.Personal.Domain.Modules.Employees.ValueObjects;

public sealed record PhoneNumber
{
    public string Value { get; }

    private PhoneNumber(string value)
    {
        Value = value;
    }

    public static PhoneNumber Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueException("Phone number cannot be empty.");

        var trimmed = value.Trim();

        if (trimmed.Length > 20)
            throw new InvalidValueException("Phone number cannot exceed 20 characters.");


        return new PhoneNumber(trimmed);
    }

    public override string ToString() => Value;
}
