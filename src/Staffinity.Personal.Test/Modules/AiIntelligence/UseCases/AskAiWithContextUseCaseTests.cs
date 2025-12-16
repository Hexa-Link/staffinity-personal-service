using Moq;
using Staffinity.Personal.Application.Modules.AiIntelligence.Contracts;
using Staffinity.Personal.Application.Modules.AiIntelligence.Services;
using Staffinity.Personal.Application.Modules.AiIntelligence.UseCases;
using Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels;
using Staffinity.Personal.Domain.Modules.AiIntelligence.Exceptions;
using Staffinity.Personal.Domain.Modules.AiIntelligence.Ports.Out;

namespace Staffinity.Personal.Test.Modules.AiIntelligence.UseCases;

public class AskAiWithContextUseCaseTests
{
    [Fact]
    public async Task EmployeeRole_ShouldThrowForbidden_AndNeverCallModel()
    {
        var intentDetector = new Mock<IIntentDetector>(MockBehavior.Strict);
        var contextBuilder = new Mock<IContextBuilder>(MockBehavior.Strict);
        var strategyRouter = new Mock<IStrategyRouter>(MockBehavior.Strict);
        var modelClient = new Mock<IAiModelClient>(MockBehavior.Strict);

        var sut = new AskAiWithContextUseCase(
            intentDetector.Object,
            contextBuilder.Object,
            strategyRouter.Object,
            modelClient.Object
        );

        var query = new AiOrchestrationQuery("How many employees do we have?", AiUserRole.Employee);

        await Assert.ThrowsAsync<ForbiddenAiDataAccessException>(() => sut.ExecuteAsync(query));

        modelClient.Verify(
            x => x.AskAsync(It.IsAny<AiModelRequest>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
    }

    [Fact]
    public async Task HrRole_ShouldBuildContext_ApplyStrategy_AndCallModel()
    {
        var intentDetector = new Mock<IIntentDetector>();
        var contextBuilder = new Mock<IContextBuilder>();
        var strategyRouter = new Mock<IStrategyRouter>();
        var modelClient = new Mock<IAiModelClient>();

        intentDetector
            .Setup(x => x.Detect(It.IsAny<string>()))
            .Returns(AiIntent.VacationRequestsOverview);

        var snapshot = AiContextSnapshot.Empty(AiIntent.VacationRequestsOverview, AiUserRole.Hr);

        contextBuilder
            .Setup(x =>
                x.BuildAsync(
                    AiIntent.VacationRequestsOverview,
                    AiUserRole.Hr,
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(snapshot);

        strategyRouter
            .Setup(x => x.BuildGuidance(AiIntent.VacationRequestsOverview))
            .Returns("GUIDANCE");

        var expectedInsight = AiInsight.CreateBasic(AiIntent.VacationRequestsOverview, "ok");

        modelClient
            .Setup(x => x.AskAsync(It.IsAny<AiModelRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedInsight);

        var sut = new AskAiWithContextUseCase(
            intentDetector.Object,
            contextBuilder.Object,
            strategyRouter.Object,
            modelClient.Object
        );

        var query = new AiOrchestrationQuery(
            "Resumen de vacaciones pendientes",
            AiUserRole.Hr,
            "corr-1"
        );

        var result = await sut.ExecuteAsync(query);

        Assert.Equal(expectedInsight, result);

        modelClient.Verify(
            x =>
                x.AskAsync(
                    It.Is<AiModelRequest>(r =>
                        r.Intent == AiIntent.VacationRequestsOverview
                        && r.RequestorRole == AiUserRole.Hr
                        && r.CorrelationId == "corr-1"
                        && r.Question.Contains("GUIDANCE")
                        && r.Question.Contains("UserQuestion:")
                        && r.Context.Intent == AiIntent.VacationRequestsOverview
                    ),
                    It.IsAny<CancellationToken>()
                ),
            Times.Once
        );
    }

    [Fact]
    public async Task InvalidIntent_ShouldThrowUnauthorizedAiIntentException()
    {
        var intentDetector = new Mock<IIntentDetector>();
        var contextBuilder = new Mock<IContextBuilder>();
        var strategyRouter = new Mock<IStrategyRouter>();
        var modelClient = new Mock<IAiModelClient>();

        // Intent invalid
        intentDetector.Setup(x => x.Detect(It.IsAny<string>())).Returns((AiIntent)999);

        var sut = new AskAiWithContextUseCase(
            intentDetector.Object,
            contextBuilder.Object,
            strategyRouter.Object,
            modelClient.Object
        );

        var query = new AiOrchestrationQuery("something", AiUserRole.Hr);

        await Assert.ThrowsAsync<UnauthorizedAiIntentException>(() => sut.ExecuteAsync(query));

        modelClient.Verify(
            x => x.AskAsync(It.IsAny<AiModelRequest>(), It.IsAny<CancellationToken>()),
            Times.Never
        );
        contextBuilder.Verify(
            x =>
                x.BuildAsync(
                    It.IsAny<AiIntent>(),
                    It.IsAny<AiUserRole>(),
                    It.IsAny<CancellationToken>()
                ),
            Times.Never
        );
    }
}
