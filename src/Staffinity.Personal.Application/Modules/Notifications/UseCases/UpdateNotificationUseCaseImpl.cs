using Staffinity.Personal.Domain.Modules.Notifications.Model;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Notifications.UseCases
{
    public class UpdateNotificationUseCaseImpl : IUpdateNotificationUseCase
    {
        INotificationRepository _notificationRepository;

        public UpdateNotificationUseCaseImpl(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public Task<Notification?> EditAsync(Notification notification)
        {
            if (notification.Id == null || notification.Id == Guid.Empty)
            {
                throw new ArgumentNullException("Notification Id cannot be null or empty");
            }

            if (notification.EmployeeId == null || notification.EmployeeId == Guid.Empty)
            {
                throw new ArgumentNullException("Employee Id cannot be null or empty");
            }

            if (String.IsNullOrEmpty(notification.Message))
            {
                throw new ArgumentNullException("Notification message cannot be null or empty");
            }

            if (notification.IsRead == null)
            {
                throw new ArgumentNullException("Is Read cannot be null");
            }

            if (String.IsNullOrEmpty(notification.RelatedUrl))
            {
                throw new ArgumentNullException("Related Url cannot be null or empty");
            }

            if (notification.SendDate == null)
            {
                throw new ArgumentNullException("Send Date cannot be null or empty");
            }

            return _notificationRepository.UpdateAsync(notification);
        }
    }
}
