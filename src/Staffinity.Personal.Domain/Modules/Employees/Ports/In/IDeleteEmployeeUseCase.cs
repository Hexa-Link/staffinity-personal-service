namespace Staffinity.Personal.Domain.Modules.Employees.Ports.In
{
        public interface IDeleteEmployeeUseCase
        {
            Task<bool> DeleteAsync(Guid employeeId);
        }

}
