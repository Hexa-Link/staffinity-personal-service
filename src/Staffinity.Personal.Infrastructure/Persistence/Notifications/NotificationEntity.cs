using System.ComponentModel.DataAnnotations;

namespace Staffinity.Personal.Infrastructure.Persistence.Notifications
{
    public class NotificationEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "EmployeeId is required")]
        public Guid EmployeeId { get; set; }

        [Required(ErrorMessage = "Message cannot be empty")]
        [MaxLength(255, ErrorMessage = "Message is too large, 255 characters maximum")]
        [MinLength(5, ErrorMessage = "Message is too short, 5 characters minimum")]
        public string Message { get; set; }

        public bool IsRead { get; set; } = false;

        [Required(ErrorMessage = "RelatedUrl is required")]
        [MaxLength(500, ErrorMessage = "RelatedUrl is too long, 500 characters maximum")]
        [Url(ErrorMessage = "RelatedUrl must be a valid URL")]
        public string RelatedUrl { get; set; }
        public DateTime SendDate { get; set; } = DateTime.UtcNow;
    }
}