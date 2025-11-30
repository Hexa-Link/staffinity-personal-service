using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Staffinity.Personal.Infrastructure.Persistence.Employees;

public class EmployeeEntity
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Phone { get; set; }
    public DateOnly BirthDate { get; set; }
    public DateOnly HireDate { get; set; }
    public Guid IdentificationTypeId { get; set; }
    public string IdentificationNumber { get; set; }
    public Guid? ManagerId { get; set; }
    public Guid HeadquartersId { get; set; }
    public Guid GenderId { get; set; }
    public Guid StatusId { get; set; }
    public Guid AccessLevelId { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}

public class EmployeeEntityConfiguration : IEntityTypeConfiguration<EmployeeEntity>
{
    public void Configure(EntityTypeBuilder<EmployeeEntity> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(e => e.Id);

        var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
            v => v.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc),
            v => DateOnly.FromDateTime(DateTime.SpecifyKind(v, DateTimeKind.Utc)));

        builder.Property(e => e.BirthDate)
            .HasConversion(dateOnlyConverter)
            .HasColumnType("date");

        builder.Property(e => e.HireDate)
            .HasConversion(dateOnlyConverter)
            .HasColumnType("date");

        builder.Property(e => e.Code).IsRequired();
        builder.Property(e => e.Name).IsRequired();
        builder.Property(e => e.Email).IsRequired();
        builder.Property(e => e.PasswordHash).IsRequired();
        builder.Property(e => e.Phone).IsRequired();
        builder.Property(e => e.IdentificationNumber).IsRequired();
    }
}
