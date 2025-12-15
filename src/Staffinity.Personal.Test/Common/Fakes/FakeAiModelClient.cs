using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;
using Staffinity.Personal.Domain.Modules.AiIntelligence.Ports.Out;

namespace Staffinity.Personal.Test.Common.Fakes;

public sealed class FakeAiModelClient : IAiModelClient
{
    public Task<AiInsight> AskAsync(
        AiModelRequest request,
        CancellationToken cancellationToken = default
    )
    {
        var insight = new AiInsight(
            Intent: request.Intent,
            Severity: AiInsightSeverity.Info,
            Summary: "Fake AI response",
            Recommendations: Array.Empty<AiRecommendation>(),
            CreatedAt: DateTimeOffset.UtcNow
        );

        return Task.FromResult(insight);
    }
}