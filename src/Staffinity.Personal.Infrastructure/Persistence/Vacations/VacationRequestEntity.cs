using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staffinity.Personal.Infrastructure.Persistence.Vacations
{
    [Table("vacation_requests")]
    public class VacationRequestEntity
    {
        [Column("vacation_request_id")]
        [Key]
        public Guid Id { get; set; }

        [Column("employee_id")]
        [Required(ErrorMessage = "EmployeeId is required")]
        public Guid EmployeeId { get; set; }

        [Column("start_date")]
        [Required]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        [Required]
        public DateTime EndDate { get; set; }

        [Column("reason")]
        [MaxLength(500, ErrorMessage = "Reason is too long, 500 characters maximum")]
        public string Reason { get; set; }

        [Column("status")]
        [Required]
        public string Status { get; set; } 

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // Constructor for EF
        public VacationRequestEntity() { }
        
        public VacationRequestEntity(Guid id, Guid employeeId, DateTime startDate, DateTime endDate, string reason, string status, DateTime createdAt)
        {
            Id = id;
            EmployeeId = employeeId;
            StartDate = startDate;
            EndDate = endDate;
            Reason = reason;
            Status = status;
            CreatedAt = createdAt;
        }
    }
}