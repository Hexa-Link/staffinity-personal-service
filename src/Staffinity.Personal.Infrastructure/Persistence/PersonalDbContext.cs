using Microsoft.EntityFrameworkCore;

namespace Staffinity.Personal.Infrastructure.Persistence;

public class PersonalDbContext(DbContextOptions<PersonalDbContext> options) : DbContext(options)
{
    //DbSets place not yet
        
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PersonalDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}