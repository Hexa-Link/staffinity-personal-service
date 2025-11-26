using Microsoft.EntityFrameworkCore;

namespace Staffinity.Personal.Infrastructure.Persistence;

public class PersonalDbContext(DbContextOptions<PersonalDbContext> options) : DbContext(options)
{
    //DbSets place not yet
        

}