using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Application.Modules.AiIntelligence.Services;

public interface IIntentDetector
{
    AiIntent Detect(string question);
}

public sealed class IntentDetector : IIntentDetector
{
    public AiIntent Detect(string question)
    {
        if (string.IsNullOrWhiteSpace(question))
            return AiIntent.HrKpiSummary;

        var q = question.ToLowerInvariant();

        if (q.Contains("vacation") || q.Contains("vacaciones") || q.Contains("leave"))
            return AiIntent.VacationRequestsOverview;

        if (
            q.Contains("headcount")
            || q.Contains("plantilla")
            || q.Contains("empleados")
            || q.Contains("employees")
        )
            return AiIntent.EmployeeHeadcountSnapshot;

        if (q.Contains("turnover") || q.Contains("rotación") || q.Contains("attrition"))
            return AiIntent.TurnoverRiskSignals;

        if (q.Contains("policy") || q.Contains("compliance") || q.Contains("cumplimiento"))
            return AiIntent.VacationPolicyCompliance;

        if (q.Contains("anomaly") || q.Contains("anomalia") || q.Contains("anomalía"))
            return AiIntent.WorkforceAnomalies;

        if (
            q.Contains("recommend")
            || q.Contains("recomienda")
            || q.Contains("acciones")
            || q.Contains("plan")
        )
            return AiIntent.ActionRecommendations;

        return AiIntent.HrKpiSummary;
    }
}
