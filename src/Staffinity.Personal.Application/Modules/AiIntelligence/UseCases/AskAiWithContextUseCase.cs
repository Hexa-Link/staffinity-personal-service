using Staffinity.Personal.Application.Modules.AiIntelligence.Contracts;
using Staffinity.Personal.Application.Modules.AiIntelligence.Services;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;
using Staffinity.Personal.Domain.Modules.AiIntelligence.Ports.Out;

namespace Staffinity.Personal.Application.Modules.AiIntelligence.UseCases;

public interface IAskAiWithContextUseCase
{
    Task<AiInsight> ExecuteAsync(
        AiOrchestrationQuery query,
        CancellationToken cancellationToken = default
    );
}

public sealed class AskAiWithContextUseCase : IAskAiWithContextUseCase
{
    private readonly IIntentDetector _intentDetector;
    private readonly IContextBuilder _contextBuilder;
    private readonly IStrategyRouter _strategyRouter;
    private readonly IAiModelClient _aiModelClient;

    public AskAiWithContextUseCase(
        IIntentDetector intentDetector,
        IContextBuilder contextBuilder,
        IStrategyRouter strategyRouter,
        IAiModelClient aiModelClient
    )
    {
        _intentDetector = intentDetector;
        _contextBuilder = contextBuilder;
        _strategyRouter = strategyRouter;
        _aiModelClient = aiModelClient;
    }

    public async Task<AiInsight> ExecuteAsync(
        AiOrchestrationQuery query,
        CancellationToken cancellationToken = default
    )
    {
        // 1) Role validation before any AI execution
        AllowedIntentsPolicy.EnsureRoleAllowed(query.RequestorRole);

        // 2) Detect intent
        var intent = _intentDetector.Detect(query.Question);

        // 3) Intent validation by role (strict control)
        AllowedIntentsPolicy.EnsureIntentAllowed(query.RequestorRole, intent);

        // 4) Context construction (aggregate)
        var context = await _contextBuilder.BuildAsync(
            intent,
            query.RequestorRole,
            cancellationToken
        );

        // 5) HR Strategy (guidance)
        var guidance = _strategyRouter.BuildGuidance(intent);

        // 6) Controlled final question (backend-only)
        var finalQuestion =
            $"{guidance}\n\n"
            + $"UserQuestion: {query.Question}\n\n"
            + $"Constraints: Use aggregated context only. No PII. No raw lists.";

        // 7) Query to the model (via Domain port)
        var request = new AiModelRequest(
            Question: finalQuestion,
            Intent: intent,
            RequestorRole: query.RequestorRole,
            Context: context,
            CorrelationId: query.CorrelationId
        );

        return await _aiModelClient.AskAsync(request, cancellationToken);
    }
}
