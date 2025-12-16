using Microsoft.EntityFrameworkCore;
using Staffinity.Personal.Application.Modules.AiIntelligence.Ports.Out;
using Staffinity.Personal.Infrastructure.Persistence;

namespace Staffinity.Personal.Infrastructure.Adapters.Ai.ContextAdapters;

public sealed class NotificationsAiContextAdapter : INotificationsAiContextPort
{
    private readonly PersonalDbContext _db;

    public NotificationsAiContextAdapter(PersonalDbContext db)
    {
        _db = db;
    }

    public async Task<NotificationsAiKpis> GetNotificationsKpisAsync(CancellationToken cancellationToken = default)
    {
        var unread = await _db.Notifications
            .CountAsync(n => !n.IsRead, cancellationToken);

        // Assuming "Pending Approvals" are notifications with specific titles or just a placeholder logic
        // For now, we'll count unread notifications that contain "Approval" in the title
        var pendingApprovals = await _db.Notifications
            .CountAsync(n => !n.IsRead && n.Title.Contains("Approval"), cancellationToken);

        return new NotificationsAiKpis(
            UnreadNotifications: unread,
            PendingApprovals: pendingApprovals
        );
    }
}
