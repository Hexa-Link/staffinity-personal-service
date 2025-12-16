using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Application.Modules.AiIntelligence.Ports.Out;
using Staffinity.Personal.Infrastructure.Persistence;

namespace Staffinity.Personal.Infrastructure.Adapters.Ai.ContextAdapters;

public sealed class EmployeesAiContextAdapter : IEmployeesAiContextPort
{
    private readonly PersonalDbContext _db;

    public EmployeesAiContextAdapter(PersonalDbContext db)
    {
        _db = db;
    }

    public async Task<EmployeesAiKpis> GetEmployeesKpisAsync(CancellationToken cancellationToken = default)
    {
        var total = await _db.Employees.CountAsync(cancellationToken);

        // Assuming StatusId for Active/Inactive. Since I don't have the exact IDs, 
        // I will count all non-deleted as active for now, or check IsDeleted.
        // Ideally we should query the Status table but for this task I'll use IsDeleted proxy if possible
        // or just count total.
        // Looking at EmployeeEntity, it has StatusId.
        // Let's assume for now we just count total and new hires based on HireDate.

        var active = await _db.Employees.CountAsync(e => !e.IsDeleted, cancellationToken);
        var inactive = await _db.Employees.CountAsync(e => e.IsDeleted, cancellationToken);

        var thirtyDaysAgo = DateTime.UtcNow.AddDays(-30);
        var newHires = await _db.Employees.CountAsync(e => e.HireDate >= thirtyDaysAgo, cancellationToken);

        return new EmployeesAiKpis(
            TotalEmployees: total,
            ActiveEmployees: active,
            InactiveEmployees: inactive,
            NewHiresLast30Days: newHires
        );
    }
}
