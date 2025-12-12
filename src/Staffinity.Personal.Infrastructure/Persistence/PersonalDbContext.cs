using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Infrastructure.Persistence.Employees;
using Staffinity.Personal.Infrastructure.Persistence.Notifications;

namespace Staffinity.Personal.Infrastructure.Persistence;

public class PersonalDbContext(DbContextOptions<PersonalDbContext> options) : DbContext(options)
{
    public DbSet<NotificationEntity> Notifications { get; set; }
    public DbSet<EmployeeEntity> Employees { get; set; }
}