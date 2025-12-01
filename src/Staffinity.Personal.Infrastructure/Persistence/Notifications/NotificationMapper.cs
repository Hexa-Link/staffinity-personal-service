using Staffinity.Personal.Application.Modules.Notifications.Dto;
using Staffinity.Personal.Domain.Modules.Notifications.Model;

namespace Staffinity.Personal.Infrastructure.Persistence.Notifications
{
    public static class NotificationMapper
    {
        public static Notification ToModel(NotificationEntity notification)
        {
            return new Notification(
                notification.Id,
                notification.RecipientId,
                notification.Title,
                notification.Message,
                notification.IsRead,
                notification.RelatedUrl,
                notification.SendDate);
        }

        public static Notification CreateRequestToModel(CreateNotificationRequest notification)
        {
            return new Notification(
                notification.RecipientId,
                notification.Title,
                notification.Message,
                notification.IsRead ?? false,
                notification.RelatedUrl,
                notification.SendDate ?? DateTime.UtcNow);
        }

        public static Notification UpdateRequestToModel(UpdateNotificationRequest notification)
        {
            return new Notification(
                notification.Id,
                notification.RecipientId,
                notification.Title,
                notification.Message,
                notification.IsRead ?? false,
                notification.RelatedUrl,
                notification.SendDate ?? DateTime.UtcNow);
        }

        public static NotificationEntity ToEntity(Notification notification)
        {
            return new NotificationEntity(
                notification.Id,
                notification.RecipientId,
                notification.Title,
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
