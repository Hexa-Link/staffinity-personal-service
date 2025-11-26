public class Notification
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public string RelatedUrl { get; set; }
    public DateTime SendDate { get; set; }

    public Notification()
    { }

    public Notification(Guid employeeId, string message, string relatedUrl)
    {
        Id = Guid.NewGuid();
        EmployeeId = employeeId;
        Message = message;
        IsRead = false;
        RelatedUrl = relatedUrl;
        SendDate = DateTime.UtcNow;
    }

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
