using System.Linq;
using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Domain.Modules.Employees.Exceptions;
using Staffinity.Personal.Domain.Modules.Employees.Model;
using Staffinity.Personal.Domain.Modules.Employees.Ports.Out;
using Staffinity.Personal.Domain.Modules.Notifications.Exceptions;

namespace Staffinity.Personal.Infrastructure.Persistence.Employees
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly PersonalDbContext _dbContext;

        public EmployeeRepository(PersonalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Employee[]> GetAllAsync()
        {
            try
            {
                var entities = await _dbContext.Employees
                    .Where(e => !e.IsDeleted)
                    .ToListAsync();
                return EmployeeMapper.ToModelList(entities);
            }
            catch (Exception)
            {
                throw new ResourceNotFoundException("No employees were found");
            }
        }

        public async Task<Employee?> GetByIdAsync(Guid employeeId)
        {
            try
            {
                var entity = await _dbContext.Employees
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == employeeId);

                if (entity is null || entity.IsDeleted)
                {
                    return null;
                }

                return EmployeeMapper.ToModel(entity);
            }
            catch (Exception)
            {
                throw new ResourceNotFoundException("The employee was not found");
            }
        }

        public async Task<Employee?> CreateAsync(Employee employee)
        {
            try
            {
                var entry = _dbContext.Employees.Add(EmployeeMapper.ToEntity(employee));
                await _dbContext.SaveChangesAsync();
                return EmployeeMapper.ToModel(entry.Entity);
            }
            catch (Exception)
            {
                throw new ResourceNotCreatedException("The employee could not be created");
            }
        }

        public async Task<Employee?> UpdateAsync(Employee employee)
        {
            try
            {
                var entity = await _dbContext.Employees.FindAsync(employee.Id);

                if (entity is null || entity.IsDeleted)
                {
                    return null;
                }

                entity.Code = employee.Code;
                entity.FirstName = employee.FirstName;
                entity.MiddleName = employee.MiddleName;
                entity.LastName = employee.LastName;
                entity.SecondLastName = employee.SecondLastName;
                entity.Email = employee.Email;
                entity.PhoneNumber = employee.PhoneNumber;
                entity.PasswordHash = employee.PasswordHash ?? entity.PasswordHash;
                entity.DateOfBirth = employee.DateOfBirth;
                entity.HireDate = employee.HireDate;
                entity.IdentificationTypeId = employee.IdentificationTypeId;
                entity.IdentificationNumber = employee.IdentificationNumber;
                entity.ManagerId = employee.ManagerId;
                entity.HeadquartersId = employee.HeadquartersId;
                entity.GenderId = employee.GenderId;
                entity.StatusId = employee.StatusId;
                entity.AccessLevelId = employee.AccessLevelId;
                entity.UpdatedAt = DateTime.UtcNow;
                entity.IsDeleted = employee.IsDeleted;

                await _dbContext.SaveChangesAsync();

                return EmployeeMapper.ToModel(entity);
            }
            catch (Exception)
            {
                throw new ResourceNotUpdatedException("The employee could not be updated");
            }
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            try
            {
                var entity = await _dbContext.Employees.FindAsync(Id);

                if (entity is null)
                {
                    return false;
                }

                entity.IsDeleted = true;
                entity.UpdatedAt = DateTime.UtcNow;

                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw new ResourceNotDeletedException("The employee could not be deleted");
            }
        }
    }
}
