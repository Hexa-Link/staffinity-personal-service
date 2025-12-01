namespace Staffinity.Personal.Domain.Modules.Notifications.Exceptions
{
    public class ResourceNotDeletedException : Exception
    {
        public ResourceNotDeletedException(string message) : base(message)
        {
        }
    }
}
