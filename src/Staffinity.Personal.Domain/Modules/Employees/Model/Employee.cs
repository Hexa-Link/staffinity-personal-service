using System;
using System.Diagnostics.CodeAnalysis;

namespace Staffinity.Personal.Domain.Modules.Employees.Model
{
    public class Employee
    {
        public Guid Id { get; set; }
        public required string Code { get; set; } = string.Empty;
        public required string Name { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string PasswordHash { get; set; } = string.Empty;
        public required string Phone { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public DateOnly HireDate { get; set; }
        public Guid IdentificationTypeId { get; set; }
        public required string IdentificationNumber { get; set; } = string.Empty;
        public Guid? ManagerId { get; set; }
        public Guid HeadquartersId { get; set; }
        public Guid GenderId { get; set; }
        public Guid StatusId { get; set; }
        public Guid AccessLevelId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public Employee()
        { }

        [SetsRequiredMembers]
        public Employee(
            string code,
            string name,
            string email,
            string passwordHash,
            string phone,
            DateOnly birthDate,
            DateOnly hireDate,
            Guid identificationTypeId,
            string identificationNumber,
            Guid? managerId,
            Guid headquartersId,
            Guid genderId,
            Guid statusId,
            Guid accessLevelId)
        {
            Id = Guid.NewGuid();
            Code = code;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Phone = phone;
            BirthDate = birthDate;
            HireDate = hireDate;
            IdentificationTypeId = identificationTypeId;
            IdentificationNumber = identificationNumber;
            ManagerId = managerId;
            HeadquartersId = headquartersId;
            GenderId = genderId;
            StatusId = statusId;
            AccessLevelId = accessLevelId;
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
            IsDeleted = false;
        }

        [SetsRequiredMembers]
        public Employee(
            Guid id,
            string code,
            string name,
            string email,
            string passwordHash,
            string phone,
            DateOnly birthDate,
            DateOnly hireDate,
            Guid identificationTypeId,
            string identificationNumber,
            Guid? managerId,
            Guid headquartersId,
            Guid genderId,
            Guid statusId,
            Guid accessLevelId,
            DateTimeOffset createdAt,
            DateTimeOffset updatedAt,
            bool isDeleted)
        {
            Id = id;
            Code = code;
            Name = name;
            Email = email;
            PasswordHash = passwordHash;
            Phone = phone;
            BirthDate = birthDate;
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
