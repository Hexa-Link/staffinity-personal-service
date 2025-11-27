using Staffinity.Personal.Domain.Modules.Employees.Exceptions;

namespace Staffinity.Personal.Domain.Modules.Employees.ValueObjects;

public sealed record EmployeeCode
{
    public string Value { get; }

    private EmployeeCode(string value)
    {
        Value = value;
    }

    public static EmployeeCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidValueException("Employee code cannot be empty.");

        var trimmed = value.Trim();

        if (trimmed.Length > 20)
            throw new InvalidValueException("Employee code cannot exceed 20 characters (max 20).");

        var normalized = trimmed.ToUpperInvariant();

        return new EmployeeCode(normalized);
    }

    public bool EqualsTo(EmployeeCode other)
    {
        if (other is null) return false;

        return Value.Equals(other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override string ToString() => Value;
}
