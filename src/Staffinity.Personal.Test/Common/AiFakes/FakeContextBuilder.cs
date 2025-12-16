using Staffinity.Personal.Application.Modules.AiIntelligence.Services;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Test.Common.AiFakes;

public sealed class FakeContextBuilder : IContextBuilder
{
    public Task<AiContextSnapshot> BuildAsync(
        AiIntent intent,
        AiUserRole role,
        CancellationToken cancellationToken = default)
    {
        return Task.FromResult(AiContextSnapshot.Empty(intent, role));
    }
}