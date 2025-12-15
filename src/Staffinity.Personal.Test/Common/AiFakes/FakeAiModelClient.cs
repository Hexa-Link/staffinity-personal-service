using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;
using Staffinity.Personal.Domain.Modules.AiIntelligence.Ports.Out;

namespace Staffinity.Personal.Test.Common.AiFakes;

public sealed class FakeAiModelClient : IAiModelClient
{
    public Task<AiInsight> AskAsync(
        AiModelRequest request,
        CancellationToken cancellationToken)
    {
        return Task.FromResult(
            AiInsight.CreateBasic(
                request.Intent,
                "This is a fake AI response for testing"
            )
        );
    }
}
