using Staffinity.Personal.Domain.Modules.Notifications.Model;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Notifications.UseCases
{
    public class GetNotificationByIdUseCaseImpl : IGetNotificationByIdUseCase
    {
        INotificationRepository _notificationRepository;

        public GetNotificationByIdUseCaseImpl(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public Task<Notification?> GetByIdAsync(Guid notificationId)
        {
            if (notificationId == null || Guid.Empty == notificationId)
            {
                throw new ArgumentNullException("Notification Id cannot be null or empty");
            }

            return _notificationRepository.GetByIdAsync(notificationId);
        }
    }
}
