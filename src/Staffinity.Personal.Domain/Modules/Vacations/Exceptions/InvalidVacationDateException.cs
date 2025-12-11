namespace Staffinity.Personal.Domain.Modules.Vacations.Exceptions;

public class InvalidVacationDateException : Exception
{
    public InvalidVacationDateException(string message) : base(message) {}
}