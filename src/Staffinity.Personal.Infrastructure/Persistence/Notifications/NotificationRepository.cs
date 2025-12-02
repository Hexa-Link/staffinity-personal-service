using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Domain.Modules.Notifications.Exceptions;
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

        public async Task<Notification[]> GetAllAsync()
        {
            try
            {
                return NotificationMapper.ToModelList(await _dbContext.Notifications.ToListAsync());
            }
            catch (Exception ex)
            {
                throw new ResourceNotFoundException("No notifications were found");
            }
        }

        public async Task<Notification?> GetByIdAsync(Guid notificationId)
        {
            try
            {
                return NotificationMapper.ToModel(await _dbContext.Notifications.FindAsync(notificationId));
            }
            catch (Exception ex)
            {
                throw new ResourceNotFoundException("The notification was not found");
            }
        }

        public async Task<Notification?> CreateAsync(Notification notification)
        {
            try
            {
                var request = _dbContext.Notifications.Add(NotificationMapper.ToEntity(notification));
                await _dbContext.SaveChangesAsync();
                return NotificationMapper.ToModel(request.Entity);
            }
            catch (Exception ex)
            {
                throw new ResourceNotCreatedException("The notification could not be created");
            }
        }

        public async Task<Notification?> UpdateAsync(Notification notification)
        {
            try
            {
                var request = _dbContext.Notifications.Update(NotificationMapper.ToEntity(notification));
                await _dbContext.SaveChangesAsync();
                return NotificationMapper.ToModel(request.Entity);
            }
            catch (Exception ex)
            {
                throw new ResourceNotUpdatedException("The notification could not be updated");
            }
        }

        public async Task<bool> DeleteAsync(Guid notificationId)
        {
            try
            {
                var notification = GetByIdAsync(notificationId);
                _dbContext.Remove(notification);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new ResourceNotDeletedException("The notification could not be deleted");
            }
        }
    }
}