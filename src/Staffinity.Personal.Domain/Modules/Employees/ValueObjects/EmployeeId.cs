using Staffinity.Personal.Domain.Modules.Employees.Exceptions;

namespace Staffinity.Personal.Domain.Modules.Employees.ValueObjects;

public sealed record EmployeeId
{
    public Guid Value { get; }

    private EmployeeId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidValueException("Employee id cannot be empty.");
        }

        Value = value;
    }

    public static EmployeeId Create(Guid value) => new(value);

    public static EmployeeId New() => new(Guid.NewGuid());

    public override string ToString() => Value.ToString();
}
