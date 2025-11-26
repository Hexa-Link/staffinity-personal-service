using Staffinity.Personal.Domain.Modules.Notifications.Model;

namespace Staffinity.Personal.Domain.Modules.Notifications.Ports.In
{
    internal interface ICreateNotificationUseCase
    {
        Task<Notification?> CreateAsync(Notification notification);
    }
}
