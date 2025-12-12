using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Domain.Modules.Vacations.Model;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.Out; 

namespace Staffinity.Personal.Infrastructure.Persistence.Vacations
{
    public class VacationRequestRepository : IVacationRequestRepository
    {
        private readonly PersonalDbContext _dbContext;

        public VacationRequestRepository(PersonalDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveAsync(VacationRequest vacationRequest)
        {
            try
            {
                var entity = VacationRequestMapper.ToEntity(vacationRequest);
                await _dbContext.Set<VacationRequestEntity>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("The vacation request could not be created", ex);
            }
        }

        public async Task UpdateAsync(VacationRequest vacationRequest)
        {
            try
            {
                var entity = VacationRequestMapper.ToEntity(vacationRequest);
                _dbContext.Set<VacationRequestEntity>().Update(entity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("The vacation request could not be updated", ex);
            }
        }

        public async Task DeleteAsync(VacationRequestId id)
        {
            try
            {
                var entity = await _dbContext.Set<VacationRequestEntity>().FindAsync(id.Value);
                if (entity != null)
                {
                    _dbContext.Set<VacationRequestEntity>().Remove(entity);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("The vacation request could not be deleted", ex);
            }
        }

        public async Task<VacationRequest?> GetByIdAsync(VacationRequestId id)
        {
            try
            {
                var entity = await _dbContext.Set<VacationRequestEntity>().FindAsync(id.Value);
                
                if (entity == null) return null;

                return VacationRequestMapper.ToModel(entity);
            }
            catch (Exception ex)
            {
                return null; 
            }
        }

        public async Task<IEnumerable<VacationRequest>> GetByEmployeeIdAsync(Guid employeeId)
        {
            try
            {
                var entities = await _dbContext.Set<VacationRequestEntity>()
                    .Where(v => v.EmployeeId == employeeId)
                    .ToListAsync();

                return VacationRequestMapper.ToModelList(entities);
            }
            catch (Exception)
            {
                return Array.Empty<VacationRequest>();
            }
        }

        public async Task<VacationRequest[]> GetAllAsync()
        {
            try
            {
                var entities = await _dbContext.Set<VacationRequestEntity>().ToListAsync();
                return VacationRequestMapper.ToModelList(entities);
            }
            catch (Exception)
            {
                return Array.Empty<VacationRequest>();
            }
        }
    }
}