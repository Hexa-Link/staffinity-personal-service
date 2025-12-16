using Staffinity.Personal.Application.Modules.AiIntelligence.Services;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Test.Common.AiFakes;

public sealed class FakeIntentDetector : IIntentDetector
{
    public AiIntent Detect(string question)
    {
        if (string.IsNullOrWhiteSpace(question))
            return AiIntent.HrKpiSummary;

        if (question.Contains("vacation", StringComparison.OrdinalIgnoreCase))
            return AiIntent.VacationRequestsOverview;

        if (question.Contains("headcount", StringComparison.OrdinalIgnoreCase))
            return AiIntent.EmployeeHeadcountSnapshot;

        return AiIntent.HrKpiSummary;
    }
}
