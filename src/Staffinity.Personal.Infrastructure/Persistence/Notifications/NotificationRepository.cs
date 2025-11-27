using Staffinity.Personal.Domain.Modules.Notifications.Model;
using Staffinity.Personal.Domain.Modules.Notifications.Ports.Out;

namespace Staffinity.Personal.Infrastructure.Persistence.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly PersonalDbContext _dbContext;

        public NotificationRepository(PersonalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<Notification[]> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Notification?> GetByIdAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }

        public Task<Notification?> CreateAsync(Notification notification)
        {
            throw new NotImplementedException();
        }

        public Task<Notification?> UpdateAsync(Notification notification)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid notificationId)
        {
            throw new NotImplementedException();
        }
    }
}