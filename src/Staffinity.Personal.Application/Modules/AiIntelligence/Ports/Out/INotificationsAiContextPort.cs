using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staffinity.Personal.Application.Modules.AiIntelligence.Ports.Out
{
    public sealed record NotificationsAiKpis(int UnreadNotifications, int PendingApprovals);

    public interface INotificationsAiContextPort
    {
        Task<NotificationsAiKpis> GetNotificationsKpisAsync(
            CancellationToken cancellationToken = default
        );
    }
}
