using System.Net.Mail;
using Staffinity.Personal.Domain.Modules.Employees.Exceptions;

namespace Staffinity.Personal.Domain.Modules.Employees.ValueObjects;

public sealed record EmployeeEmail
{
    public string Value { get; }

    private EmployeeEmail(string value)
    {
        Value = value;
    }

    public static EmployeeEmail Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEmailException(string.Empty);

        var trimmed = value.Trim();

        if (trimmed.Length > 60)
            throw new InvalidEmailException($"Email exceeds maximum allowed length of 60 characters: '{trimmed}'");

        if (!IsValid(trimmed))
            throw new InvalidEmailException(trimmed);

        var normalized = trimmed.ToLowerInvariant();

        return new EmployeeEmail(normalized);
    }

    private static bool IsValid(string value)
    {
        try
        {
            var address = new MailAddress(value);

            return string.Equals(address.Address, value, StringComparison.OrdinalIgnoreCase);
        }
        catch
        {
            return false;
        }
    }

    public override string ToString() => Value;

    public bool EqualsTo(EmployeeEmail other)
    {
        if (other is null) return false;
        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }
}
