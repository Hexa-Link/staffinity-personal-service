using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Staffinity.Personal.Application.Modules.AiIntelligence.Ports.Out
{
    public sealed record EmployeesAiKpis(
        int TotalEmployees,
        int ActiveEmployees,
        int InactiveEmployees,
        int NewHiresLast30Days
    );

    public interface IEmployeesAiContextPort
    {
        Task<EmployeesAiKpis> GetEmployeesKpisAsync(CancellationToken cancellationToken = default);
    }
}
