namespace Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels
{
    public enum AiIntent
    {
        // Executive summaries and aggregated metrics
        HrKpiSummary = 0,
        EmployeeHeadcountSnapshot = 1,
        VacationRequestsOverview = 2,

        // Strategic insights (without PII / without raw listings)
        TurnoverRiskSignals = 10,
        WorkforceAnomalies = 11,
        VacationPolicyCompliance = 12,

        // Actionable recommendations (business)
        ActionRecommendations = 20,
    }
}
