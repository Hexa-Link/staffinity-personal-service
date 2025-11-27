namespace Staffinity.Personal.Domain.Modules.Employees.Exceptions;

public class InvalidEmailException : Exception
{
    public InvalidEmailException(string email)
        : base($"Email '{email}' is not valid.")
    {
    }
}

public class EmptyNameException : Exception
{
    public EmptyNameException()
        : base("Employee name cannot be empty.")
    {
    }
}

public class InvalidValueException : Exception
{
    public InvalidValueException(string message)
        : base(message)
    {
    }
}
