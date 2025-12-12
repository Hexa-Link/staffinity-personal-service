
namespace Staffinity.Personal.Application.Modules.Vacations.Dto;

public class CreateVacationRequestDto
{
    public Guid EmployeeId {get; set;}
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = string.Empty;
}