using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Domain.Modules.Vacations.Model;
using Staffinity.Personal.Domain.Modules.Vacations.Ports.Out; 

namespace Staffinity.Personal.Infrastructure.Persistence.Vacations
{
    public class VacationRequestRepository(PersonalDbContext dbContext) : IVacationRequestRepository
    {
        public async Task SaveAsync(VacationRequest vacationRequest)
        {
            try
            {
                var entity = VacationRequestMapper.ToEntity(vacationRequest);
                await dbContext.Set<VacationRequestEntity>().AddAsync(entity);
                await dbContext.SaveChangesAsync();
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
                dbContext.Set<VacationRequestEntity>().Update(entity);
                await dbContext.SaveChangesAsync();
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
                var entity = await dbContext.Set<VacationRequestEntity>().FindAsync(id.Value);
                if (entity != null)
                {
                    dbContext.Set<VacationRequestEntity>().Remove(entity);
                    await dbContext.SaveChangesAsync();
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
                var entity = await dbContext.Set<VacationRequestEntity>().FindAsync(id.Value);
                
                if (entity == null) return null;

                return VacationRequestMapper.ToModel(entity);
            }
            catch (Exception)
            {
                return null; 
            }
        }

        public async Task<IEnumerable<VacationRequest>> GetByEmployeeIdAsync(Guid employeeId)
        {
            try
            {
                var entities = await dbContext.Set<VacationRequestEntity>()
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
                var entities = await dbContext.Set<VacationRequestEntity>().ToListAsync();
                return VacationRequestMapper.ToModelList(entities);
            }
            catch (Exception)
            {
                return Array.Empty<VacationRequest>();
            }
        }
    }
}