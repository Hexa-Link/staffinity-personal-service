using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Domain.Modules.AiIntelligence.Ports.Out;

public sealed record AiModelRequest(
    string Question,
    AiIntent Intent,
    AiUserRole RequestorRole,
    AiContextSnapshot Context,
    string? CorrelationId = null
);

public interface IAiModelClient
{
    Task<AiInsight> AskAsync(AiModelRequest request, CancellationToken cancellationToken = default);
}
