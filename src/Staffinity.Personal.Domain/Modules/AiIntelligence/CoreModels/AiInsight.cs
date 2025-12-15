namespace Staffinity.Personal.Domain.Modules.AiIntelligence.CoreModels
{
    public enum AiInsightSeverity
    {
        Info = 0,
        Warning = 1,
        Critical = 2,
    }

    public sealed record AiRecommendation(
        string Title,
        string Rationale,
        string? SuggestedAction = null
    );

    public sealed record AiInsight(
        AiIntent Intent,
        AiInsightSeverity Severity,
        string Summary,
        IReadOnlyList<AiRecommendation> Recommendations,
        DateTimeOffset CreatedAt
    )
    {
        public static AiInsight CreateBasic(AiIntent intent, string summary) =>
            new(
                Intent: intent,
                Severity: AiInsightSeverity.Info,
                Summary: summary,
                Recommendations: Array.Empty<AiRecommendation>(),
                CreatedAt: DateTimeOffset.UtcNow
            );
    }
}
