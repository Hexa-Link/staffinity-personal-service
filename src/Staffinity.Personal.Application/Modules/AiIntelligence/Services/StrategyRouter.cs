using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Application.Modules.AiIntelligence.Services;

public interface IStrategyRouter
{
    string BuildGuidance(AiIntent intent);
}

public sealed class StrategyRouter : IStrategyRouter
{
    public string BuildGuidance(AiIntent intent) =>
        intent switch
        {
            AiIntent.HrKpiSummary =>
                "You are an HR business assistant. Provide a concise executive summary using ONLY the provided metrics. Do not request or infer PII.",

            AiIntent.EmployeeHeadcountSnapshot =>
                "Explain workforce snapshot trends and implications. Use aggregated metrics only. Provide 2-3 actionable HR insights.",

            AiIntent.VacationRequestsOverview =>
                "Summarize vacation workload and risks (pending, upcoming approvals). Suggest operational actions to reduce backlog.",

            AiIntent.TurnoverRiskSignals =>
                "Identify possible turnover risk signals from aggregated indicators. Provide preventative HR actions without naming individuals.",

            AiIntent.WorkforceAnomalies =>
                "Detect anomalies or unusual patterns from aggregated metrics. Suggest what to investigate next without accessing raw data.",

            AiIntent.VacationPolicyCompliance =>
                "Assess vacation policy compliance risks using aggregated context. Suggest policy/process adjustments.",

            AiIntent.ActionRecommendations =>
                "Provide prioritized action recommendations (max 5) grounded in the context metrics, with rationale and expected impact.",

            _ => "You are an HR business assistant. Use aggregated context only.",
        };
}
