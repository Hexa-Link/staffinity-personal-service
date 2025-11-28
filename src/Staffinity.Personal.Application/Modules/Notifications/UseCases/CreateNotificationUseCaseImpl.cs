using Staffinity.Personal.Domain.Modules.Notifications.Model;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Notifications.UseCases
{
    public class CreateNotificationUseCaseImpl : ICreateNotificationUseCase
    {
        INotificationRepository _notificationRepository;

        public CreateNotificationUseCaseImpl(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public Task<Notification?> CreateAsync(Notification notification)
        {
            if (notification.EmployeeId == null || notification.EmployeeId == Guid.Empty)
            {
                throw new ArgumentNullException("Employee Id cannot be null or empty");
            }

            if (String.IsNullOrEmpty(notification.Message))
            {
                throw new ArgumentNullException("Notification message cannot be null or empty");
            }

            if (String.IsNullOrEmpty(notification.RelatedUrl))
            {
                throw new ArgumentNullException("Related Url cannot be null or empty");
            }

            return _notificationRepository.CreateAsync(notification);
        }
    }
}
