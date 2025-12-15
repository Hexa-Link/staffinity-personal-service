using Staffinity.Personal.Application.Modules.AiIntelligence.Services;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;

namespace Staffinity.Personal.Test.Common.AiFakes;

public sealed class FakeStrategyRouter : IStrategyRouter
{
    public string BuildGuidance(AiIntent intent)
        => "Provide high-level HR guidance.";
}
