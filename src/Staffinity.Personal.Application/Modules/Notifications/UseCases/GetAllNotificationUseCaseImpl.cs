using Staffinity.Personal.Domain.Modules.Notifications.Model;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.In;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;

namespace Staffinity.Personal.Application.Modules.Notifications.UseCases
{
    public class GetAllNotificationUseCaseImpl : IGetAllNotificationsUseCase
    {
        INotificationRepository _notificationRepository;

        GetAllNotificationUseCaseImpl(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        public async Task<Notification[]> GetAllAsync()
        {

            return await _notificationRepository.GetAllAsync();
        }
    }
}
