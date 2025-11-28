using Staffinity.Personal.Domain.Modules.Notifications.Model;

namespace Staffinity.Personal.Infrastructure.Persistence.Notifications
{
    internal static class NotificationMapper
    {
        public static Notification ToModel(NotificationEntity notification)
        {
            return new Notification(
                notification.Id,
                notification.EmployeeId,
                notification.Message,
                notification.IsRead,
                notification.RelatedUrl,
                notification.SendDate);
        }

        public static NotificationEntity ToEntity(Notification notification)
        {
            return new NotificationEntity(
                notification.Id,
                notification.EmployeeId,
                notification.Message,
                notification.IsRead,
                notification.RelatedUrl,
                notification.SendDate);
        }

        public static Notification[] ToModelList(List<NotificationEntity> notifications)
        {
            return notifications.Select(n => ToModel(n)).ToArray();
        }

        public static NotificationEntity[] ToEntityList(List<Notification> notifications)
        {
            return notifications.Select(n => ToEntity(n)).ToArray();
        }
    }
}
