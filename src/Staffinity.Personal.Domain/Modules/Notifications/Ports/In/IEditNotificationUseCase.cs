using Staffinity.Personal.Domain.Modules.Notifications.Model;

namespace Staffinity.Personal.Domain.Modules.Notifications.Ports.In
{
    internal interface IEditNotificationUseCase
    {
        Task<bool> EditAsync(Notification notification);
    }
}
