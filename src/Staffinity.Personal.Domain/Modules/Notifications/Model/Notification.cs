using System.Diagnostics.CodeAnalysis;

namespace Staffinity.Personal.Domain.Modules.Notifications.Model
{
    public class Notification
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public required string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public required string RelatedUrl { get; set; } = string.Empty;
        public DateTime SendDate { get; set; }

        public Notification()
        { }

        [SetsRequiredMembers]
        public Notification(Guid employeeId, string message, string relatedUrl)
        {
            Id = Guid.NewGuid();
            EmployeeId = employeeId;
            Message = message;
            IsRead = false;
            RelatedUrl = relatedUrl;
            SendDate = DateTime.UtcNow;
        }

        [SetsRequiredMembers]
        public Notification(Guid id, Guid employeeId, string message, bool isRead, string relatedUrl, DateTime sendDate)
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
