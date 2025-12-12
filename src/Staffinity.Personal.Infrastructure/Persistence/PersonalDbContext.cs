using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Infrastructure.Persistence.Notifications;
using Staffinity.Personal.Infrastructure.Persistence.Vacations;

namespace Staffinity.Personal.Infrastructure.Persistence;

public class PersonalDbContext(DbContextOptions<PersonalDbContext> options) : DbContext(options)
{
    public DbSet<NotificationEntity> Notifications { get; set; }
    public DbSet<VacationRequestEntity> VacationRequests { get; set; }
}