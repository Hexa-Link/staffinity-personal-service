using Staffinity.Personal.Domain.Modules.Notifications.Model;

namespace Staffinity.Personal.Domain.Modules.Notifications.Ports.In
{
    internal interface IEditNotificationUseCase
    {
        Task<Notification?> EditAsync(Notification notification);
    }
}
