namespace Staffinity.Personal.Domain.Modules.Employees.Model
{
    public class Employee
    {
        public Guid Id { get; set; }

        public string Code { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string? SecondLastName { get; set; }

        public string Email { get; set; } = string.Empty;
        public string? PasswordHash { get; set; } = string.Empty;

        public string? PhoneNumber { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime HireDate { get; set; }

        public Guid IdentificationTypeId { get; set; }
        public string IdentificationNumber { get; set; } = string.Empty;

        public Guid? ManagerId { get; set; }
        public Guid? HeadquartersId { get; set; }
        public Guid GenderId { get; set; }
        public Guid StatusId { get; set; }
        public Guid AccessLevelId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public Employee()
        { }

        public Employee(
            string code,
            string firstName,
            string? middleName,
            string lastName,
            string? secondLastName,
            string email,
            string passwordHash,
            string? phoneNumber,
            DateTime dateOfBirth,
            DateTime hireDate,
            Guid identificationTypeId,
            string identificationNumber,
            Guid? managerId,
            Guid? headquartersId,
            Guid genderId,
            Guid statusId,
            Guid accessLevelId)
        {
            Id = Guid.NewGuid();
            Code = code;

            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            SecondLastName = secondLastName;

            Email = email;
            PasswordHash = passwordHash;
            PhoneNumber = phoneNumber;

            DateOfBirth = dateOfBirth;
            HireDate = hireDate;

            IdentificationTypeId = identificationTypeId;
            IdentificationNumber = identificationNumber;

            ManagerId = managerId;
            HeadquartersId = headquartersId;
            GenderId = genderId;
            StatusId = statusId;
            AccessLevelId = accessLevelId;

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
            IsDeleted = false;
        }

        public Employee(
            Guid id,
            string code,
            string firstName,
            string? middleName,
            string lastName,
            string? secondLastName,
            string email,
            string? passwordHash,
            string? phoneNumber,
            DateTime dateOfBirth,
            DateTime hireDate,
            Guid identificationTypeId,
            string identificationNumber,
            Guid? managerId,
            Guid? headquartersId,
            Guid genderId,
            Guid statusId,
            Guid accessLevelId,
            DateTime createdAt,
            bool isDeleted)
        {
            Id = id;
            Code = code;

            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            SecondLastName = secondLastName;

            Email = email;
            PasswordHash = passwordHash;
            PhoneNumber = phoneNumber;

            DateOfBirth = dateOfBirth;
            HireDate = hireDate;

            IdentificationTypeId = identificationTypeId;
            IdentificationNumber = identificationNumber;

            ManagerId = managerId;
            HeadquartersId = headquartersId;
            GenderId = genderId;
            StatusId = statusId;
            AccessLevelId = accessLevelId;

            CreatedAt = createdAt;
            UpdatedAt = DateTime.UtcNow;
            IsDeleted = isDeleted;
        }

        public Employee(
            Guid id,
            string code,
            string firstName,
            string? middleName,
            string lastName,
            string? secondLastName,
            string email,
            string? passwordHash,
            string? phoneNumber,
            DateTime dateOfBirth,
            DateTime hireDate,
            Guid identificationTypeId,
            string identificationNumber,
            Guid? managerId,
            Guid? headquartersId,
            Guid genderId,
            Guid statusId,
            Guid accessLevelId,
            DateTime createdAt,
            DateTime updatedAt,
            bool isDeleted)
        {
            Id = id;
            Code = code;

            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
            SecondLastName = secondLastName;

            Email = email;
            PasswordHash = passwordHash;
            PhoneNumber = phoneNumber;

            DateOfBirth = dateOfBirth;
            HireDate = hireDate;

            IdentificationTypeId = identificationTypeId;
            IdentificationNumber = identificationNumber;

            ManagerId = managerId;
            HeadquartersId = headquartersId;
            GenderId = genderId;
            StatusId = statusId;
            AccessLevelId = accessLevelId;

            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            IsDeleted = isDeleted;
        }
    }
}
