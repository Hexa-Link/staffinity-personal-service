using Staffinity.Personal.Domain.Modules.Employees.Model;

namespace Staffinity.Personal.Infrastructure.Persistence.Employees
{
    public static class EmployeeMapper
    {
        public static Employee ToModel(EmployeeEntity entity)
        {
            return new Employee(
                entity.Id,
                entity.Code,
                entity.FirstName,
                entity.MiddleName,
                entity.LastName,
                entity.SecondLastName,
                entity.Email,
                entity.PasswordHash ?? "",
                entity.PhoneNumber,
                entity.DateOfBirth,
                entity.HireDate,
                entity.IdentificationTypeId,
                entity.IdentificationNumber,
                entity.ManagerId,
                entity.HeadquartersId,
                entity.GenderId,
                entity.StatusId,
                entity.AccessLevelId,
                entity.CreatedAt ?? DateTime.UtcNow,
                entity.UpdatedAt ?? DateTime.UtcNow,
                entity.IsDeleted
            );
        }

        public static EmployeeEntity ToEntity(Employee employee)
        {
            return new EmployeeEntity
            {
                Id = employee.Id,
                Code = employee.Code,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                LastName = employee.LastName,
                SecondLastName = employee.SecondLastName,
                Email = employee.Email,
                PasswordHash = employee.PasswordHash,
                PhoneNumber = employee.PhoneNumber,
                DateOfBirth = employee.DateOfBirth,
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

        public static Employee[] ToModelList(List<EmployeeEntity> entities)
        {
            return entities.Select(ToModel).ToArray();
        }

        public static EmployeeEntity[] ToEntityList(List<Employee> employees)
        {
            return employees.Select(ToEntity).ToArray();
        }
    }
}
