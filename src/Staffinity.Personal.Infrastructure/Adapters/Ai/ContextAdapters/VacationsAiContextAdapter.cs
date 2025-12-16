using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Application.Modules.AiIntelligence.Ports.Out;
using Staffinity.Personal.Infrastructure.Persistence;

namespace Staffinity.Personal.Infrastructure.Adapters.Ai.ContextAdapters;

public sealed class VacationsAiContextAdapter : IVacationsAiContextPort
{
    private readonly PersonalDbContext _db;

    public VacationsAiContextAdapter(PersonalDbContext db)
    {
        _db = db;
    }

    public async Task<VacationsAiKpis> GetVacationsKpisAsync(CancellationToken cancellationToken = default)
    {
        var pending = await _db.VacationRequests
            .CountAsync(v => v.Status == "Pending", cancellationToken);

        var today = DateTime.UtcNow;
        var next30Days = today.AddDays(30);

        var upcoming = await _db.VacationRequests
            .CountAsync(v => v.Status == "Approved" && v.StartDate >= today && v.StartDate <= next30Days, cancellationToken);

        var rejected = await _db.VacationRequests
            .CountAsync(v => v.Status == "Rejected" && v.CreatedAt >= today.AddDays(-30), cancellationToken);

        return new VacationsAiKpis(
            PendingRequests: pending,
            ApprovedUpcomingNext30Days: upcoming,
            RejectedLast30Days: rejected
        );
    }
}
