namespace Staffinity.Personal.Application.Modules.Notifications.Dto
{
    public class UpdateNotificationRequest
    {

        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string RelatedUrl { get; set; }
        public DateTime SendDate { get; set; }
    }
}
