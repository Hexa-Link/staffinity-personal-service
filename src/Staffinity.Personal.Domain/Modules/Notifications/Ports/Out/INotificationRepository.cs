using Staffinity.Personal.Domain.Modules.Notifications.Model;

namespace Staffinity.Personal.Domain.Modules.Notifications.Ports.Out
{
    public interface INotificationRepository
    {
        Task<Notification[]> GetAllAsync();
        Task<Notification?> GetByIdAsync(Guid notificationId);
        Task<Notification?> CreateAsync(Notification notification);
        Task<Notification?> UpdateAsync(Notification notification);
        Task<bool> DeleteAsync(Guid notificationId);
    }
}
