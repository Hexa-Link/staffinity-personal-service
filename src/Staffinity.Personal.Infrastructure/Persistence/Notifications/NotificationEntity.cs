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
        [Required(ErrorMessage = "RecipientId is required")]
        public Guid RecipientId { get; set; }

        [Column("message")]
        [Required(ErrorMessage = "Message cannot be empty")]
        [MaxLength(255, ErrorMessage = "Message is too large, 255 characters maximum")]
        [MinLength(5, ErrorMessage = "Message is too short, 5 characters minimum")]
        public string Message { get; set; }

        [Column("title")]
        [Required(ErrorMessage = "Title cannot be empty")]
        [MaxLength(255, ErrorMessage = "Title is too large, 255 characters maximum")]
        [MinLength(5, ErrorMessage = "Title is too short, 5 characters minimum")]
        public string Title { get; set; }

        [Column("is_read")]
        public bool IsRead { get; set; } = false;

        [Column("related_url")]
        [MaxLength(255, ErrorMessage = "RelatedUrl is too long, 255 characters maximum")]
        public string? RelatedUrl { get; set; }

        [Column("sent_date")]
        public DateTime SendDate { get; set; } = DateTime.UtcNow;

        public NotificationEntity()
        { }

        public NotificationEntity(Guid recipientId, string title, string message, string relatedUrl)
        {
            Id = Guid.NewGuid();
            RecipientId = recipientId;
            Title = title;
            Message = message;
            IsRead = false;
            RelatedUrl = relatedUrl;
            SendDate = DateTime.UtcNow;
        }

        public NotificationEntity(Guid id, Guid recipientId, string title, string message, bool isRead, string relatedUrl, DateTime sendDate)
        {
            Id = id;
            RecipientId = recipientId;
            Title = title;
            Message = message;
            IsRead = isRead;
            RelatedUrl = relatedUrl;
            SendDate = sendDate;
        }
    }
}