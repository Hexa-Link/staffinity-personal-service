using Staffinity.Personal.Domain.Modules.Vacations.Exceptions;

namespace Staffinity.Personal.Domain.Modules.Vacations.Model;

public class VacationRequest
{
    public VacationRequestId Id { get; private set; }
    public Guid EmployeeId { get; private set; }
    public DateTime StartDate { get; private set; }
    public DateTime EndDate { get; private set; }
    public string Reason { get; private set; }
    public VacationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Constructor private for EFC
    private VacationRequest()
    {
    }

    // Public constructor to create a new request
    public VacationRequest(VacationRequestId id, Guid employeeId, DateTime startDate, DateTime endDate, string reason)
    {
        ValidateDates(startDate, endDate);

        Id = id;
        EmployeeId = employeeId;
        StartDate = startDate;
        EndDate = endDate;
        Reason = reason;
        Status = VacationStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }
    // Public constructor to load history load
    public VacationRequest(VacationRequestId id, Guid employeeId, DateTime startDate, DateTime endDate, string reason,
        VacationStatus status, DateTime createdAt)
    {
        Id = id;
        EmployeeId = employeeId;
        StartDate = startDate;
        EndDate = endDate;
        Reason = reason;
        Status = status;
        CreatedAt = createdAt;
    }


    // Comportment: Validate dates
    private void ValidateDates(DateTime start, DateTime end)
    {
        if (start < DateTime.UtcNow.Date)
        {
            throw new InvalidVacationDateException("The start date cannot be in the past.");
        }

        if (end < start)
        {
            throw new InvalidVacationDateException("The end date must be after the start date.");
        }
    }

    // Comportment: Approved
    public void Approve()
    {
        if (Status != VacationStatus.Pending)
        {
            throw new InvalidOperationException("Only pending requests can be approved.");
        }

        Status = VacationStatus.Approved;
    }

    // Comportment : Reject
    public void Reject()
    {
        if (Status != VacationStatus.Pending)
        {
            throw new InvalidOperationException("Only pending requests can be rejected.");
        }

        Status = VacationStatus.Rejected;
    }

    // Comportment: Cancel
    public void Cancel()
    {
        if (Status == VacationStatus.Rejected)
        {
            throw new InvalidOperationException("You cannot cancel a request that has already been rejected.");
        }

        if (Status == VacationStatus.Cancelled)
        {
            throw new  InvalidOperationException("You cannot cancel a request that has already been cancelled.");
        }
        
        Status = VacationStatus.Cancelled;
    }
}