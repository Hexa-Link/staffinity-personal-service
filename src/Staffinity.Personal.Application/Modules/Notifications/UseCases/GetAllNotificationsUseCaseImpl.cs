using Staffinity.Personal.Domain.Modules.Notifications.Model;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Notifications.UseCases
{
    public class GetAllNotificationsUseCaseImpl : IGetAllNotificationsUseCase
    {
        INotificationRepository _notificationRepository;

        public GetAllNotificationsUseCaseImpl(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification[]> GetAllAsync()
        {

            return await _notificationRepository.GetAllAsync();
        }
    }
}
