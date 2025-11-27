using Staffinity.Personal.Domain.Modules.Employees.Exceptions;
using Staffinity.Personal.Domain.Modules.Employees.ValueObjects;

namespace Staffinity.Personal.Domain.Modules.Employees.Model;

public sealed class Employee
{
    public EmployeeId Id { get; }
    public EmployeeCode Code { get; private set; }
    public PersonName Name { get; private set; }
    public EmployeeEmail Email { get; private set; }
    public string PasswordHash { get; private set; }
    public PhoneNumber Phone { get; private set; }
    public DateOnly BirthDate { get; private set; }
    public DateOnly HireDate { get; private set; }
    public Guid IdentificationTypeId { get; private set; }
    public string IdentificationNumber { get; private set; }
    public EmployeeId? ManagerId { get; private set; }
    public Guid HeadquartersId { get; private set; }
    public Guid GenderId { get; private set; }
    public Guid StatusId { get; private set; }
    public Guid AccessLevelId { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset UpdatedAt { get; private set; }
    public bool IsDeleted { get; private set; }

    private Employee(
        EmployeeId id,
        EmployeeCode code,
        PersonName name,
        EmployeeEmail email,
        string passwordHash,
        PhoneNumber phone,
        DateOnly birthDate,
        DateOnly hireDate,
        Guid identificationTypeId,
        string identificationNumber,
        EmployeeId? managerId,
        Guid headquartersId,
        Guid genderId,
        Guid statusId,
        Guid accessLevelId,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt,
        bool isDeleted)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Code = code ?? throw new ArgumentNullException(nameof(code));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Email = email ?? throw new ArgumentNullException(nameof(email));
        PasswordHash = EnsureNotEmpty(passwordHash, "Password hash cannot be empty.");
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
        BirthDate = birthDate;
        HireDate = hireDate;
        IdentificationTypeId = EnsureGuid(identificationTypeId, "Identification type id cannot be empty.");
        IdentificationNumber = EnsureNotEmpty(identificationNumber, "Identification number cannot be empty.");
        ManagerId = managerId;
        HeadquartersId = EnsureGuid(headquartersId, "Headquarters id cannot be empty.");
        GenderId = EnsureGuid(genderId, "Gender id cannot be empty.");
        StatusId = EnsureGuid(statusId, "Status id cannot be empty.");
        AccessLevelId = EnsureGuid(accessLevelId, "Access level id cannot be empty.");
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        IsDeleted = isDeleted;
    }

    public static Employee Create(
        EmployeeId id,
        EmployeeCode code,
        PersonName name,
        EmployeeEmail email,
        string passwordHash,
        PhoneNumber phone,
        DateOnly birthDate,
        DateOnly hireDate,
        Guid identificationTypeId,
        string identificationNumber,
        EmployeeId? managerId,
        Guid headquartersId,
        Guid genderId,
        Guid statusId,
        Guid accessLevelId,
        DateTimeOffset createdAt,
        DateTimeOffset updatedAt,
        bool isDeleted = false)
    {
        return new Employee(
            id,
            code,
            name,
            email,
            passwordHash,
            phone,
            birthDate,
            hireDate,
            identificationTypeId,
            identificationNumber,
            managerId,
            headquartersId,
            genderId,
            statusId,
            accessLevelId,
            createdAt,
            updatedAt,
            isDeleted);
    }

    public void ChangeName(PersonName name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public void ChangeEmail(EmployeeEmail email)
    {
        Email = email ?? throw new ArgumentNullException(nameof(email));
    }

    public void ChangePhone(PhoneNumber phone)
    {
        Phone = phone ?? throw new ArgumentNullException(nameof(phone));
    }

    public void MarkDeleted(DateTimeOffset deletedAt)
    {
        IsDeleted = true;
        UpdatedAt = deletedAt;
    }

    public void UpdatePassword(string passwordHash, DateTimeOffset updatedAt)
    {
        PasswordHash = EnsureNotEmpty(passwordHash, "Password hash cannot be empty.");
        UpdatedAt = updatedAt;
    }

    public void UpdateEmail(EmployeeEmail email, DateTimeOffset updatedAt)
    {
        ChangeEmail(email);
        UpdatedAt = updatedAt;
    }

    public void UpdatePhone(PhoneNumber phone, DateTimeOffset updatedAt)
    {
        ChangePhone(phone);
        UpdatedAt = updatedAt;
    }

    private static Guid EnsureGuid(Guid value, string message)
    {
        if (value == Guid.Empty)
        {
            throw new InvalidValueException(message);
        }

        return value;
    }

    private static string EnsureNotEmpty(string value, string message)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidValueException(message);
        }

        return value.Trim();
    }
}
