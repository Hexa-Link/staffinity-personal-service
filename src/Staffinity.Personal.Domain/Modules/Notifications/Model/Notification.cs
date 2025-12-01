namespace Staffinity.Personal.Domain.Modules.Notifications.Model
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid RecipientId { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; } = false;
        public string? RelatedUrl { get; set; }
        public DateTime SendDate { get; set; } = DateTime.UtcNow;

        public Notification()
        { }

        public Notification(Guid recipientId, string title, string message, string relatedUrl)
        {
            Id = Guid.NewGuid();
            RecipientId = recipientId;
            Title = title;
            Message = message;
            IsRead = false;
            RelatedUrl = relatedUrl;
            SendDate = DateTime.UtcNow;
        }

        public Notification(Guid recipientId, string title, string message, bool isRead, string relatedUrl, DateTime sendDate)
        {
            RecipientId = recipientId;
            Title = title;
            Message = message;
            IsRead = isRead;
            RelatedUrl = relatedUrl;
            SendDate = sendDate;
        }

        public Notification(Guid id, Guid recipientId, string title, string message, bool isRead, string relatedUrl, DateTime sendDate)
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