using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Staffinity.Personal.Infrastructure.Persistence.Notifications
{
    [Table("notifications")]
    public class NotificationEntity
    {
        [Column("notification_id")]
        [Key]
        public Guid Id { get; set; }

        [Column("recipient_id")]
        [Required(ErrorMessage = "EmployeeId is required")]
        public Guid EmployeeId { get; set; }

        [Column("message")]
        [Required(ErrorMessage = "Message cannot be empty")]
        [MaxLength(255, ErrorMessage = "Message is too large, 255 characters maximum")]
        [MinLength(5, ErrorMessage = "Message is too short, 5 characters minimum")]
        public string Message { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; } = false;

        [Column("related_url")]
        [MaxLength(500, ErrorMessage = "RelatedUrl is too long, 500 characters maximum")]
        public string? RelatedUrl { get; set; }

        [Column("sent_date")]
        public DateTime SendDate { get; set; } = DateTime.UtcNow;

        public NotificationEntity()
        { }

        public NotificationEntity(Guid employeeId, string message, string relatedUrl)
        {
            Id = Guid.NewGuid();
            EmployeeId = employeeId;
            Message = message;
            IsRead = false;
            RelatedUrl = relatedUrl;
            SendDate = DateTime.UtcNow;
        }

        public NotificationEntity(Guid id, Guid employeeId, string message, bool isRead, string relatedUrl, DateTime sendDate)
        {
            Id = id;
            EmployeeId = employeeId;
            Message = message;
            IsRead = isRead;
            RelatedUrl = relatedUrl;
            SendDate = sendDate;
        }
    }
}