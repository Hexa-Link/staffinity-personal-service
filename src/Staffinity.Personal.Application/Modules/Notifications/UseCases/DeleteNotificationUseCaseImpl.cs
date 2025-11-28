using Staffinity.Personal.Domain.Modules.Notifications.Exceptions;
using Staffinity.Personal.Domain.Modules.Notifications.Model;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Notifications.UseCases
{
    public class DeleteNotificationUseCaseImpl : IDeleteNotificationUseCase
    {
        INotificationRepository _notificationRepository;

        DeleteNotificationUseCaseImpl(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public Task<bool> DeleteAsync(Guid notificationId)
        {
            if (notificationId == null || Guid.Empty == notificationId)
            {
                throw new ArgumentNullException("Notification Id cannot be null or empty");
            }

            return _notificationRepository.DeleteAsync(notificationId);
        }
    }
}
