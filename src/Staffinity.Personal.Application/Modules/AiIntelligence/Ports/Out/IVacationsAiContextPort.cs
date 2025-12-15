using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staffinity.Personal.Application.Modules.AiIntelligence.Ports.Out
{
    public sealed record VacationsAiKpis(
        int PendingRequests,
        int ApprovedUpcomingNext30Days,
        int RejectedLast30Days
    );

    public interface IVacationsAiContextPort
    {
        Task<VacationsAiKpis> GetVacationsKpisAsync(CancellationToken cancellationToken = default);
    }
}
