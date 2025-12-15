using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Staffinity.Personal.Infrastructure.Persistence.Employees;

[Table("employees")]
public class EmployeeEntity
{
    [Key]
    [Column("employee_id")]
    public Guid Id { get; set; }

    [Column("employee_code")]
    [Required(ErrorMessage = "Employee code is required")]
    [MaxLength(20, ErrorMessage = "Employee code cannot exceed 20 characters")]
    public string Code { get; set; } = string.Empty;

    [Column("first_name")]
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(40, ErrorMessage = "First name cannot exceed 40 characters")]
    public string FirstName { get; set; } = string.Empty;

    [Column("middle_name")]
    [MaxLength(40, ErrorMessage = "Middle name cannot exceed 40 characters")]
    public string? MiddleName { get; set; }

    [Column("last_name")]
    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(40, ErrorMessage = "Last name cannot exceed 40 characters")]
    public string LastName { get; set; } = string.Empty;

    [Column("second_last_name")]
    [MaxLength(40, ErrorMessage = "Second last name cannot exceed 40 characters")]
    public string? SecondLastName { get; set; }

    [Column("email")]
    [Required(ErrorMessage = "Email is required")]
    [MaxLength(60, ErrorMessage = "Email cannot exceed 60 characters")]
    public string Email { get; set; } = string.Empty;

    [Column("password_hash")]
    [MaxLength(80, ErrorMessage = "Password hash cannot exceed 80 characters")]
    public string? PasswordHash { get; set; } = string.Empty;

    [Column("phone_number")]
    [MaxLength(20, ErrorMessage = "Phone number cannot exceed 20 characters")]
    public string? PhoneNumber { get; set; }

    [Column("identification_number")]
    [Required(ErrorMessage = "Identification number is required")]
    [MaxLength(50, ErrorMessage = "Identification number cannot exceed 50 characters")]
    public string IdentificationNumber { get; set; } = string.Empty;

    [Column("date_of_birth")]
    [Required(ErrorMessage = "Date of birth is required")]
    public DateTime DateOfBirth { get; set; }

    [Column("hire_date")]
    [Required(ErrorMessage = "Hire date is required")]
    public DateTime HireDate { get; set; }

    [Column("gender_id")]
    [Required(ErrorMessage = "Gender is required")]
    public Guid GenderId { get; set; }

    [Column("identification_type_id")]
    [Required(ErrorMessage = "Identification type is required")]
    public Guid IdentificationTypeId { get; set; }

    [Column("headquarters_id")]
    [Required(ErrorMessage = "Headquarters is required")]
    public Guid HeadquartersId { get; set; }

    [Column("access_level_id")]
    [Required(ErrorMessage = "Access level is required")]
    public Guid AccessLevelId { get; set; }

    [Column("status_id")]
    [Required(ErrorMessage = "Status is required")]
    public Guid StatusId { get; set; }

    [Column("manager_id")]
    public Guid? ManagerId { get; set; }

    [Column("is_deleted")]
    public bool IsDeleted { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
}

public class EmployeeEntityConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.ToTable("employees");

        builder.HasKey(e => e.Id);

        var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
            v => v.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            v => DateOnly.FromDateTime(DateTime.SpecifyKind(v, DateTimeKind.Utc)));

        builder.Property(e => e.DateOfBirth)
            .HasConversion(dateOnlyConverter)
            .HasColumnType("date");

        builder.Property(e => e.HireDate)
            .HasConversion(dateOnlyConverter)
            .HasColumnType("date");

        builder.Property(e => e.Code).IsRequired();
        builder.Property(e => e.FirstName).IsRequired();
        builder.Property(e => e.LastName).IsRequired();
        builder.Property(e => e.Email).IsRequired();
        builder.Property(e => e.PasswordHash).IsRequired();
        builder.Property(e => e.IdentificationNumber).IsRequired();

        builder.Property(e => e.CreatedAt)
            .HasDefaultValueSql("NOW()");

        builder.Property(e => e.UpdatedAt)
            .HasDefaultValueSql("NOW()");
    }
}
