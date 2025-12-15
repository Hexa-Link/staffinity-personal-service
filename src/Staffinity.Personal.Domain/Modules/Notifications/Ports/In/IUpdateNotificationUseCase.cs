using Staffinity.Personal.Domain.Modules.Notifications.Model;

namespace Staffinity.Personal.Domain.Modules.Notifications.Ports.In
{
    public interface IUpdateNotificationUseCase
    {
        Task<Notification?> EditAsync(Notification notification);
    }
}
