namespace Staffinity.Personal.Domain.Modules.Notifications.Ports.In
{
    public interface IDeleteNotificationUseCase
    {
        Task<bool> DeleteAsync(Guid notificationId);
    }
}
