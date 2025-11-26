namespace Staffinity.Personal.Domain.Modules.Notifications.Exceptions
{
    internal class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string message) : base(message)
        {
        }
    }
}
