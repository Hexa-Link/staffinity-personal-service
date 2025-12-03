using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;
using AppDbContext = Staffinity.Personal.Infrastructure.Persistence.PersonalDbContext;

namespace Staffinity.Personal.Infrastructure.Persistence.Employees;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly AppDbContext _context;
    private DbSet<EmployeeEntity> Employees => _context.Set<EmployeeEntity>();

    public EmployeeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Employee[]> GetAllAsync()
    {
        var entities = await Employees.AsNoTracking().ToArrayAsync();
        return entities.Select(MapToDomain).ToArray();
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        var entity = await Employees.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
        return entity == null ? null : MapToDomain(entity);
    }

    public async Task<Employee?> CreateAsync(Employee employee)
    {
        var entity = MapToEntity(employee);
        await Employees.AddAsync(entity);
        await _context.SaveChangesAsync();
        return MapToDomain(entity);
    }

    public async Task<Employee?> UpdateAsync(Employee employee)
    {
        var entity = await Employees.FirstOrDefaultAsync(e => e.Id == employee.Id);
        if (entity == null)
        {
            return null;
        }

        entity.Code = employee.Code;
        entity.Name = employee.Name;
        entity.Email = employee.Email;
        entity.PasswordHash = employee.PasswordHash;
        entity.Phone = employee.Phone;
        entity.BirthDate = employee.BirthDate;
        entity.HireDate = employee.HireDate;
        entity.IdentificationTypeId = employee.IdentificationTypeId;
        entity.IdentificationNumber = employee.IdentificationNumber;
        entity.ManagerId = employee.ManagerId;
        entity.HeadquartersId = employee.HeadquartersId;
        entity.GenderId = employee.GenderId;
        entity.StatusId = employee.StatusId;
        entity.AccessLevelId = employee.AccessLevelId;
        entity.CreatedAt = employee.CreatedAt;
        entity.UpdatedAt = employee.UpdatedAt;
        entity.IsDeleted = employee.IsDeleted;

        await _context.SaveChangesAsync();
        return MapToDomain(entity);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await Employees.FirstOrDefaultAsync(e => e.Id == id);
        if (entity == null)
        {
            return false;
        }

        Employees.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    private static Employee MapToDomain(EmployeeEntity entity)
    {
        return new Employee(
            entity.Id,
            entity.Code,
            entity.Name,
            entity.Email,
            entity.PasswordHash,
            entity.Phone,
            entity.BirthDate,
            entity.HireDate,
            entity.IdentificationTypeId,
            entity.IdentificationNumber,
            entity.ManagerId,
            entity.HeadquartersId,
            entity.GenderId,
            entity.StatusId,
            entity.AccessLevelId,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.IsDeleted);
    }

    private static EmployeeEntity MapToEntity(Employee employee)
    {
        return new EmployeeEntity
        {
            Id = employee.Id,
            Code = employee.Code,
            Name = employee.Name,
            Email = employee.Email,
            PasswordHash = employee.PasswordHash,
            Phone = employee.Phone,
            BirthDate = employee.BirthDate,
            HireDate = employee.HireDate,
            IdentificationTypeId = employee.IdentificationTypeId,
            IdentificationNumber = employee.IdentificationNumber,
            ManagerId = employee.ManagerId,
            HeadquartersId = employee.HeadquartersId,
            GenderId = employee.GenderId,
            StatusId = employee.StatusId,
            AccessLevelId = employee.AccessLevelId,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt,
            IsDeleted = employee.IsDeleted
        };
    }
}
