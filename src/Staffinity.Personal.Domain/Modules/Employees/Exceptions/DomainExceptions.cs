namespace Staffinity.Personal.Domain.Modules.Employees.Exceptions
{
    public class InvalidValueException : Exception
    {
        public InvalidValueException(string message)
            : base(message)
        {
        }
    }
}
