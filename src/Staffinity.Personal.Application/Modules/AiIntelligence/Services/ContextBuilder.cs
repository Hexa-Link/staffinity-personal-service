using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Staffinity.Personal.Application.Modules.AiIntelligence.Ports.Out;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Application.Modules.AiIntelligence.Services;

public interface IContextBuilder
{
    Task<AiContextSnapshot> BuildAsync(
        AiIntent intent,
        AiUserRole role,
        CancellationToken cancellationToken = default
    );
}

public sealed class ContextBuilder : IContextBuilder
{
    private readonly IEmployeesAiContextPort _employees;
    private readonly IVacationsAiContextPort _vacations;
    private readonly INotificationsAiContextPort _notifications;

    public ContextBuilder(
        IEmployeesAiContextPort employees,
        IVacationsAiContextPort vacations,
        INotificationsAiContextPort notifications
    )
    {
        _employees = employees;
        _vacations = vacations;
        _notifications = notifications;
    }

    public async Task<AiContextSnapshot> BuildAsync(
        AiIntent intent,
        AiUserRole role,
        CancellationToken cancellationToken = default
    )
    {
        var employeesTask = _employees.GetEmployeesKpisAsync(cancellationToken);
        var vacationsTask = _vacations.GetVacationsKpisAsync(cancellationToken);
        var notificationsTask = _notifications.GetNotificationsKpisAsync(cancellationToken);

        await Task.WhenAll(employeesTask, vacationsTask, notificationsTask);

        var e = employeesTask.Result;
        var v = vacationsTask.Result;
        var n = notificationsTask.Result;

        var metrics = new List<AiMetric>
        {
            new("employees.total", e.TotalEmployees, "count"),
            new("employees.active", e.ActiveEmployees, "count"),
            new("employees.inactive", e.InactiveEmployees, "count"),
            new("employees.new_hires_30d", e.NewHiresLast30Days, "count"),
            new("vacations.pending_requests", v.PendingRequests, "count"),
            new("vacations.approved_upcoming_30d", v.ApprovedUpcomingNext30Days, "count"),
            new("vacations.rejected_30d", v.RejectedLast30Days, "count"),
            new("notifications.unread", n.UnreadNotifications, "count"),
            new("notifications.pending_approvals", n.PendingApprovals, "count"),
        };

        var tags = new Dictionary<string, string>
        {
            ["context.version"] = "v1",
            ["context.scope"] = "aggregated-only",
            ["context.sources"] = "employees,vacations,notifications",
        };

        return new AiContextSnapshot(
            CapturedAt: DateTimeOffset.UtcNow,
            Intent: intent,
            RequestorRole: role,
            Metrics: metrics,
            Tags: tags
        );
    }
}
