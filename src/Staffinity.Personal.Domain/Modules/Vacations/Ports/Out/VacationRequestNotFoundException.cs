namespace Staffinity.Personal.Domain.Modules.Vacations.Ports.Out
{
    public class VacationRequestNotFoundException : Exception
    {
        public VacationRequestNotFoundException(string message) : base(message)
        {}
    }
}